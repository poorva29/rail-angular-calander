using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using System.Configuration;
using DAL;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Model;
using MiraiConsultMVC.Models.User;
using log4net;
namespace MiraiConsultMVC.Controllers
{
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/
        _dbAskMiraiDataContext db;
        BasePage BPage = new BasePage();
        private static readonly ILog logfile = LogManager.GetLogger(typeof(QuestionsController));
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DoctorQuestionList(int userId = 0, string questionsType = "")
        {
            BasePage BPage = new BasePage();
            int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.questionlist));
            if (privilege == 1)
            {
                return RedirectToAction("NoPrivilegeError", "Home");
            }
            else
            {
                Session["UnQuestionCount"] = showUnansweredQuestionCount();
                if (Session["userid"] != null)
                {
                    userId = Convert.ToInt32(Session["userid"].ToString());
                }
                IList<QuestionModel> Questions = new List<QuestionModel>();

                db = new _dbAskMiraiDataContext();
                var QuestionsById = db.getQuestionListByDoctorid(userId).ToList();
                QuestionModel QModel;
                AnswerModel AModel;
                bool filter = false;
                if (questionsType.Contains("Unanswered") || string.IsNullOrEmpty(questionsType))
                {
                    filter = true;
                }
                if (QuestionsById != null && QuestionsById.Count > 0)
                {
                    foreach (var question in QuestionsById)
                    {
                        QModel = new QuestionModel();
                        AModel = new AnswerModel();
                        QModel.QuestionId = Convert.ToInt32(question.questionid);
                        QModel.QuestionText = Convert.ToString(question.questiontext);
                        QModel.CreateDate = Convert.ToDateTime(question.createdate);
                        QModel.QuestionTextSeo = question.question_seo;
                        if (question.answeredby != null)
                        {
                            QModel.AnsweredBy = Convert.ToInt32(question.answeredby);
                        }
                        if (question.answertext != null)
                        {
                            AModel.AnswerText = Convert.ToString(question.answertext);
                            QModel.answers.Add(AModel);
                        }
                        QModel.UserId = userId;
                        QModel.Filter = filter;
                        Questions.Add(QModel);
                    }
                }
                else
                {
                    QModel = new QuestionModel();
                    AModel = new AnswerModel();
                    QModel.QuestionId = 0;
                    QModel.Filter = filter;
                    Questions.Add(QModel);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_DoctorQuestionList", Questions);
                }
                return View(Questions);
            }
        }
        public ActionResult PreRegistrationUser(int QuestionId, string email)
        {
            try
            {
                _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
                Login login = new Login();
                User user = new User();
                string password = string.Empty;
                int status;
                int userType;
                bool IsEmailVerified;
                Random random = new Random();
                password = Convert.ToString(random.Next(100000, 999999));
                password = Utilities.Encrypt("Mirai" + password);
                status = Convert.ToInt32(UserStatus.Approved);
                userType = Convert.ToInt32(UserType.Doctor);
                IsEmailVerified = true;
                var result = UserManager.getInstance().GetPreRegistrationUser(email, password, status, userType, IsEmailVerified, QuestionId);
                if (result.Email != null)
                {
                    user.UserId = result.UserId;
                    if (result.IsUserRegistered == false)
                    {
                        string doctorName = result.FirstName + " " + result.LastName;
                        string tempPassword = Utilities.Decrypt(password);
                        string emailBody = EmailTemplates.GetEmailTemplateToSendWelcomeMessage(doctorName, result.Email, tempPassword);
                        string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                        string Logoimage = Server.MapPath(@"~/Content/image/LogoForMail.png");
                        Mail.SendHTMLMailWithImage(fromEmail, result.Email, "MiraiConsult - Your Mirai Consult account has been created", emailBody, Logoimage);
                    }
                    login.IsUserRegistered = result.IsUserRegistered;
                    login.Email = result.Email;
                    login.Password = result.Password;
                    login.IsCampainUser = true;
                    login.QuestionId = QuestionId;
                    return RedirectToAction("CampaignLogin", "User", new { Username = result.Email, Password = result.Password, QuestionId = QuestionId });
                }
            }
            catch(Exception e)
            {
                logfile.Error("Controller exception >>> \n" + e.Message);
            }
            return RedirectToAction("Login", "User");
        }
        [HttpGet]
        public ActionResult DoctorQuestionDetails(string seoQuestionText = null)
        {
            try
            {
                db = new _dbAskMiraiDataContext();
                int QuestionId = 0;
                if (!String.IsNullOrEmpty(seoQuestionText))
                {
                    question q = db.questions.FirstOrDefault(x => x.question_seo.Equals(seoQuestionText));
                    if (q != null)
                        QuestionId = q.questionid;
                }
                Session["UnQuestionCount"] = showUnansweredQuestionCount();
                TempData["QuestionId"] = QuestionId;
                int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.doctorquestiondetails));
                //if (privilege == 1)
                //{
                //    return RedirectToAction("NoPrivilegeError", "Home");
                //}
                //else
                //{
                    int userId = Convert.ToInt32(Session["UserId"]);
                    IList<QuestionDtlModel> QDModel = new List<QuestionDtlModel>();
                    QuestionDtlModel qm;
                    db = new _dbAskMiraiDataContext();
                    System.Data.Linq.ISingleResult<get_questiondetailsbyIdResult> ModelQuestion = db.get_questiondetailsbyId(QuestionId, userId, 0, 1);
                    foreach (var item in ModelQuestion)
                    {
                        qm = new QuestionDtlModel();
                        qm.AnswerDate = Convert.ToDateTime(item.answerdate);
                        qm.AnswerId = Convert.ToInt32(item.answerid);
                        qm.AnswerImg = item.answerimg;
                        qm.AnswerText = item.answertext;
                        qm.CreateDate = Convert.ToDateTime(item.createdate);
                        qm.DocconnectDoctorId = item.docconnectdoctorid;
                        qm.DocId = Convert.ToInt32(item.Docid);
                        qm.Doctor = item.doctor;
                        qm.DoctorImg = item.doctorimg;
                        qm.Email = item.Email;
                        qm.EndorseCount = Convert.ToInt32(item.endorsecount);
                        qm.Gender = Convert.ToInt32(item.gender);
                        qm.Id = item.id;
                        qm.IsDocconnectUser = Convert.ToBoolean(item.isdocconnectuser);
                        qm.IsEndorse = Convert.ToBoolean(item.isendorse);
                        qm.IsPatientThank = Convert.ToBoolean(item.ispatientthank);
                        qm.LastName = item.lastname;
                        qm.MobileNo = item.mobileno;
                        qm.PatientEmail = item.patientemail;
                        qm.PatientLastName = item.patientlastname;
                        qm.QuestionId = Convert.ToInt32(item.questionid);
                        qm.QuestionText = item.questiontext;
                        qm.status = Convert.ToInt32(item.status);
                        qm.ThanxCount = Convert.ToInt32(item.thanxcount);
                        qm.Title = item.title;
                        qm.UserId = Convert.ToInt32(item.userid);
                        qm.Name_seo = item.name_seo;
                        QDModel.Add(qm);
                    }
                    return View(QDModel);
                }
            //}
            catch(Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DoctorQuestionDetails(FormCollection formCollection, HttpPostedFileBase file)
        {
            IList<QuestionDtlModel> QDModel = new List<QuestionDtlModel>();
            if (ModelState.IsValid)
            {
                int userId = 0;
                DataSet QuestionDetails;
                int QuestionId = Convert.ToInt32(TempData["QuestionId"]);
                TempData["QuestionId"] = QuestionId; //when user refresh page then action gets QuestionId
                if (Session["UserId"] != null)
                {
                    userId = Convert.ToInt32(Session["UserId"].ToString());
                }
                file = Request.Files["AnswerImg"];
                string filename = file.FileName;
                filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                if (!string.IsNullOrEmpty(filename))
                {
                    filename = Convert.ToString(QuestionId) + Convert.ToString(userId) + filename;
                }
                string answer = formCollection["answerText"]; ;
                string title = "";
                if (!String.IsNullOrEmpty(formCollection["Title"]))
                {
                    title = formCollection["Title"];
                }
                int result = QuestionManager.getInstance().SaveDoctorAnswer(QuestionId, userId, title, answer, filename);
                if (result > 0)
                {
                    if (result == 1)
                        ViewBag.lblSuccessMessage = "Thank you for answering the question.";
                    else
                        ViewBag.lblSuccessMessage = null;
                    QuestionDtlModel qm;
                    db = new _dbAskMiraiDataContext();
                    System.Data.Linq.ISingleResult<get_questiondetailsbyIdResult> ModelQuestion = db.get_questiondetailsbyId(QuestionId, userId, 0, 1);
                    QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyId(QuestionId, userId, Convert.ToInt32(QuestionStatus.Approved));
                    foreach (var item in ModelQuestion)
                    {
                        qm = new QuestionDtlModel();
                        qm.AnswerDate = Convert.ToDateTime(item.answerdate);
                        qm.AnswerId = Convert.ToInt32(item.answerid);
                        qm.AnswerImg = item.answerimg;
                        qm.AnswerText = item.answertext;
                        qm.CreateDate = Convert.ToDateTime(item.createdate);
                        qm.DocconnectDoctorId = item.docconnectdoctorid;
                        qm.DocId = Convert.ToInt32(item.Docid);
                        qm.Doctor = item.doctor;
                        qm.DoctorImg = item.doctorimg;
                        qm.Email = item.Email;
                        qm.EndorseCount = Convert.ToInt32(item.endorsecount);
                        qm.Gender = Convert.ToInt32(item.gender);
                        qm.Id = item.id;
                        qm.IsDocconnectUser = Convert.ToBoolean(item.isdocconnectuser);
                        qm.IsEndorse = Convert.ToBoolean(item.isendorse);
                        qm.IsPatientThank = Convert.ToBoolean(item.ispatientthank);
                        qm.LastName = item.lastname;
                        qm.MobileNo = item.mobileno;
                        qm.PatientEmail = item.patientemail;
                        qm.PatientLastName = item.patientlastname;
                        qm.QuestionId = Convert.ToInt32(item.questionid);
                        qm.QuestionText = item.questiontext;
                        qm.status = Convert.ToInt32(item.status);
                        qm.ThanxCount = Convert.ToInt32(item.thanxcount);
                        qm.Title = item.title;
                        qm.UserId = Convert.ToInt32(item.userid);
                        qm.Name_seo = item.name_seo;
                        QDModel.Add(qm);
                    }
                    if (filename != "")
                    {
                        string strPhysicalFilePath = "";
                        string[] array = { ".PNG", ".JPG", ".GIF", ".JPEG" };
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (filename.EndsWith(array[i]))
                            {
                                filename = filename.Replace(array[i], array[i].ToLower());
                                break;
                            }
                        }
                        string ImageUpoading_path = ConfigurationManager.AppSettings["AnswerPhotosPath"].ToString().Trim();
                        string onlyFile = filename.Substring(filename.LastIndexOf('\\') + 1);
                        if (ImageUpoading_path != "")
                        {
                            strPhysicalFilePath = ImageUpoading_path + @"\" + onlyFile;
                            if (!Directory.Exists(ImageUpoading_path.Trim()))
                            {
                                Directory.CreateDirectory(ImageUpoading_path.Trim());
                            }
                            if (!System.IO.File.Exists(strPhysicalFilePath))
                            {
                                var path = Path.Combine(ImageUpoading_path, filename);
                                file.SaveAs(path);
                            }
                            else
                            {
                                System.IO.File.Delete(strPhysicalFilePath);
                                var path = Path.Combine(ImageUpoading_path, filename);
                                file.SaveAs(path);
                            }
                        }
                    }
                    for (int i = 0; i < QuestionDetails.Tables[0].Rows.Count; i++)
                    {
                        if (QuestionDetails.Tables[0].Rows[i]["DocId"].ToString() == Convert.ToString(userId))
                        {
                            Session["UnQuestionCount"] = showUnansweredQuestionCount();
                            string msgText = ConfigurationManager.AppSettings["OnDocAnswerAssignQuestionSendEmail"].ToString();
                            string emailBody = EmailTemplates.GetEmailTemplateOnQuestionAnswer(msgText, QuestionDetails.Tables[0].Rows[i]["lastname"].ToString(), QuestionDetails.Tables[0].Rows[i]["questiontext"].ToString(), QuestionDetails.Tables[0].Rows[i]["answertext"].ToString());
                            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                            string Logoimage = Server.MapPath(@"~/Content/image/LogoForMail.png");
                            Mail.SendHTMLMailWithImage(fromEmail, QuestionDetails.Tables[0].Rows[i]["Email"].ToString(), "Mirai Consult - Answer Notification", emailBody, Logoimage);
                            string BookAppointmentUrl = ConfigurationManager.AppSettings["BookAppointmentLink"].ToString();
                            string Patientemailbody = EmailTemplates.GetEmailTemplateOnQuestionAnswerToPatient(QuestionDetails.Tables[0].Rows[i]["patientlastname"].ToString(), QuestionDetails.Tables[0].Rows[i]["lastname"].ToString(), QuestionDetails.Tables[0].Rows[i]["questiontext"].ToString(), BookAppointmentUrl, QuestionDetails.Tables[0].Rows[i]["DocId"].ToString());
                            Mail.SendHTMLMailWithImage(fromEmail, QuestionDetails.Tables[0].Rows[i]["patientemail"].ToString(), "Mirai Consult - The doctor answered your question", Patientemailbody, Logoimage);
                            string SmsText = ConfigurationManager.AppSettings["OnDocAnswerQuestionSendSMS"].ToString();
                            SMS.SendSMS(QuestionDetails.Tables[0].Rows[i]["mobileno"].ToString(), SmsText);
                        }
                    }
                }
            }
            return View(QDModel);
        }
        private int showUnansweredQuestionCount()
        {
            if (Session["UserId"] != null && Session["UserType"] != null && Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Doctor))
            {
                return QuestionManager.getInstance().getUnansweredQuestionCount(Convert.ToInt32(Session["UserId"]));
            }
            return 0;
        }
        public ActionResult QuestionDetails(int QuestionId)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            IList<QuestionDtlModel> IListQuestionDetails = new List<QuestionDtlModel>();
            QuestionDtlModel QuestionDetail;
            db = new _dbAskMiraiDataContext();
            System.Data.Linq.ISingleResult<get_questiondetailsbyIdResult> ModelQuestion = db.get_questiondetailsbyId(QuestionId, userId, 0, 1);
            foreach (var item in ModelQuestion)
            {
                QuestionDetail = new QuestionDtlModel();
                QuestionDetail.AnswerDate = Convert.ToDateTime(item.answerdate);
                QuestionDetail.AnswerId = Convert.ToInt32(item.answerid);
                QuestionDetail.AnswerImg = item.answerimg;
                QuestionDetail.AnswerText = item.answertext;
                QuestionDetail.CreateDate = Convert.ToDateTime(item.createdate);
                QuestionDetail.DocconnectDoctorId = item.docconnectdoctorid;
                QuestionDetail.DocId = Convert.ToInt32(item.Docid);
                QuestionDetail.Doctor = item.doctor;
                QuestionDetail.DoctorImg = item.doctorimg;
                QuestionDetail.Email = item.Email;
                QuestionDetail.EndorseCount = Convert.ToInt32(item.endorsecount);
                QuestionDetail.Gender = Convert.ToInt32(item.gender);
                QuestionDetail.Id = item.id;
                QuestionDetail.IsDocconnectUser = Convert.ToBoolean(item.isdocconnectuser);
                QuestionDetail.IsEndorse = Convert.ToBoolean(item.isendorse);
                QuestionDetail.IsPatientThank = Convert.ToBoolean(item.ispatientthank);
                QuestionDetail.LastName = item.lastname;
                QuestionDetail.MobileNo = item.mobileno;
                QuestionDetail.PatientEmail = item.patientemail;
                QuestionDetail.PatientLastName = item.patientlastname;
                QuestionDetail.QuestionId = Convert.ToInt32(item.questionid);
                QuestionDetail.QuestionText = item.questiontext;
                QuestionDetail.status = Convert.ToInt32(item.status);
                QuestionDetail.ThanxCount = Convert.ToInt32(item.thanxcount);
                QuestionDetail.Title = item.title;
                QuestionDetail.UserId = Convert.ToInt32(item.userid);
                IListQuestionDetails.Add(QuestionDetail);
            }
            ViewBag.AskmiraiUrl = Convert.ToString(ConfigurationSettings.AppSettings["askMiraiLink"]);
            ViewBag.FacebookAppKey = Convert.ToString(ConfigurationSettings.AppSettings["FacebookAppKey"]);
            return View(IListQuestionDetails);
        }
    }
}
