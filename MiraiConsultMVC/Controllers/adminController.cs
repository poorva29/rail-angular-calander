using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using System.Data;
using DAL;
using System.Configuration;
using MiraiConsultMVC.Models.admin;
using System.Data.SqlClient;

namespace MiraiConsultMVC.Controllers
{
    public class adminController : Controller
    {
        //
        // GET: /admin/
        public int questionId;
        private int userId;
        public DataSet QuestionDetails;
        public DataTable AssignDoctors;
        int assignQuestion = 1;
        _dbAskMiraiDataContext db;
        public ActionResult assignquestion(int? QuestionId)
        {

            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt32(Session["UserId"].ToString());
            }
            QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyId(Convert.ToInt32(QuestionId), userId, assignQuestion, Convert.ToInt32(QuestionStatus.Approved));
            
            AssignDoctors = QuestionDetails.Tables[2];


            DataTable dtTags = UtilityManager.getInstance().getAlltags();

            List<tag> tags = new List<tag>();

            tags = dtTags.AsEnumerable().Select(dataRow => new tag
                {
                    tagid = dataRow.Field<int>("tagid"),
                    tagname = dataRow.Field<string>("tagname"),
                }).ToList();

            if (QuestionDetails != null && QuestionDetails.Tables.Count != 0 && QuestionDetails.Tables[1].Rows.Count != 0)
            {
                List<tag> Selectedtags = new List<tag>();
                Selectedtags = QuestionDetails.Tables[1].AsEnumerable().Select(dataRow => new tag
                {
                    tagid = dataRow.Field<int>("tagid")
                    
                }).ToList();

                MultiSelectList makeSelected = new MultiSelectList(tags, "tagid", "tagname", Selectedtags);
                ViewBag.tags = makeSelected;
            }

            

                List<AssignQuestion> viewmodel = new List<AssignQuestion>();

                viewmodel = AssignDoctors.AsEnumerable().Select(dataRow => new AssignQuestion
                {
                    id = dataRow.Field<int>("id"),
                    name = dataRow.Field<string>("name"),
                    cities = dataRow.Field<string>("cities"),
                    specialities = dataRow.Field<string>("specialities"),
                    userid = dataRow.Field<int>("userid"),
                    locations = dataRow.Field<string>("locations"),
                    questiontext = QuestionDetails.Tables[0].Rows[0]["questiontext"].ToString(),
                    Questionid = Convert.ToInt32(QuestionId)
                }).ToList();

                return View(viewmodel);
        }

        public JsonResult AssignDoctor(int QuestionId, string AssignDoctorIds)
        {
            string[] ArrayOfID = null;
            
            questionId = QuestionId;
            AssignDoctors = QuestionManager.getInstance().assignDoctorToQuestion(questionId, AssignDoctorIds).Tables[0];

            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt32(Session["UserId"].ToString());
            }
            QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyId(Convert.ToInt32(QuestionId), userId, assignQuestion, Convert.ToInt32(QuestionStatus.Approved));

            if (AssignDoctors != null && AssignDoctors.Rows.Count != 0)
            {
                if (!String.IsNullOrEmpty(AssignDoctorIds))
                {
                    ArrayOfID = AssignDoctorIds.Split(',');
                }
                for (int i = 0; i < AssignDoctors.Rows.Count; i++)
                {
                    if (ArrayOfID.Contains(Convert.ToString(AssignDoctors.Rows[i]["userid"])))
                    {
                        string msgText = ConfigurationManager.AppSettings["OnDocAssignQuestionSendEmail"].ToString();
                        if (AssignDoctors.Rows[i]["lastname"] != System.DBNull.Value && QuestionDetails.Tables[0].Rows[0]["questiontext"] != System.DBNull.Value && AssignDoctors.Rows[i]["mobileno"] != System.DBNull.Value)
                        {
                            string emailBody = EmailTemplates.GetEmailTemplateOnQuestionAssign(msgText, Convert.ToString(AssignDoctors.Rows[i]["lastname"]), QuestionDetails.Tables[0].Rows[0]["questiontext"].ToString());
                            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                            string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                            Mail.SendHTMLMailWithImage(fromEmail, Convert.ToString(AssignDoctors.Rows[i]["email"]), "Mirai Consult - Assigned one question to you", emailBody, Logoimage);
                            string SmsText = ConfigurationManager.AppSettings["OnDocAssignQuestionSendSMS"].ToString();
                            SMS.SendSMS(Convert.ToString(AssignDoctors.Rows[i]["mobileno"]), SmsText);
                        }
                    }
                }
            }

            List<AssignQuestion> viewmodel = new List<AssignQuestion>();

            viewmodel = AssignDoctors.AsEnumerable().Select(dataRow => new AssignQuestion
            {
                id = dataRow.Field<int>("id"),
                name = dataRow.Field<string>("name"),
                cities = dataRow.Field<string>("cities"),
                specialities = dataRow.Field<string>("specialities"),
                userid = dataRow.Field<int>("userid"),
                locations = dataRow.Field<string>("locations"),
                questiontext = QuestionDetails.Tables[0].Rows[0]["questiontext"].ToString(),
                Questionid = Convert.ToInt32(QuestionId),
            }).ToList();


            return Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuestionList(bool filter = true)
        {
            List<QuestionModel> Qmodel = new List<QuestionModel>();
            Qmodel.Add(new QuestionModel { Filter = filter, AnsweredBy = filter ? 1 : 0, Counts = ConfigurationManager.AppSettings["NumberOfRecoredonQuestionList"].ToString() });
            return View(Qmodel);
        }

        public JsonResult RejectQuestionByQuestionID(int qusetionID)
        {
            SqlConnection conn = null;
            string jsonObj;
            int QusetionID = qusetionID;
            int statusRejected = (int)QuestionStatus.Rejected;
            DataTable dtUserDetails = null;
            dtUserDetails = QuestionManager.getInstance().RejectQuestionFromQuestionList(QusetionID, statusRejected);
            string mobileno = Convert.ToString(dtUserDetails.Rows[0]["mobileno"]);
            string email = Convert.ToString(dtUserDetails.Rows[0]["email"]);
            if (dtUserDetails != null)
            {
                string emailText = ConfigurationManager.AppSettings["OnRemoveQuestionFromListEmail"].ToString();
                string emailBody = EmailTemplates.GetEmailTemplateOnRejectQuestionFromQuestionList(emailText, Convert.ToString(dtUserDetails.Rows[0]["firstname"] + " " + dtUserDetails.Rows[0]["lastname"]));
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                string Logoimage = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\image\\LogoForMail.png";
                Mail.SendHTMLMailWithImage(fromEmail, Convert.ToString(dtUserDetails.Rows[0]["email"]), "Mirai Consult - Your question has been rejected", emailBody, Logoimage);
                string SmsText = ConfigurationManager.AppSettings["OnRemoveQuestionFromListSMS"].ToString();
                SMS.SendSMS(Convert.ToString(dtUserDetails.Rows[0]["mobileno"]), SmsText);
                jsonObj = "Question has been Rejected successfully.";
                
            }
            else
            {
                jsonObj = "Unable to change the status.";
            }
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }

        public JsonResult RemoveAssignDoctorToQuetion(string userid, string questionId)
        {
            string msg = "";
            int result = QuestionManager.getInstance().RemoveAssignDoctorbyUserID(Convert.ToInt32(userid), Convert.ToInt32(questionId));
            if (result >= 1)
            {
                msg = "Assign doctor has been removed successfully.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                msg = "Unable to removed the assign doctor .";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
