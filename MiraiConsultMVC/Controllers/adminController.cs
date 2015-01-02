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
using Model;
using MiraiConsultMVC;

namespace MiraiConsultMVC.Controllers
{
    public class adminController : Controller
    {
        BasePage BPage = new BasePage();
        // GET: /admin/
        public int questionId;
        private int userId;
        public DataSet QuestionDetails;
        public DataTable AssignDoctors;
        int assignQuestion = 1;
        _dbAskMiraiDataContext db;

        public ActionResult assignquestion(int? QuestionId = 0, string seoQuestionText = null)
        {
            int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.assignQuestion));
            if (privilege == 1)
            {
                return RedirectToAction("NoPrivilegeError", "Home");
            }
            else
            {
                db = new _dbAskMiraiDataContext();
                if (!string.IsNullOrEmpty(seoQuestionText) && QuestionId == 0)
                {
                    QuestionId = db.questions.FirstOrDefault(x => x.question_seo.Equals(seoQuestionText)) != null ? db.questions.FirstOrDefault(x => x.question_seo.Equals(seoQuestionText)).questionid : 0;
                }
                ViewBag.questionid = QuestionId;
                if (Session["UserId"] != null)
                {
                    userId = Convert.ToInt32(Session["UserId"].ToString());
                }
                QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyId(Convert.ToInt32(QuestionId), userId, assignQuestion, Convert.ToInt32(QuestionStatus.Approved));

                if (QuestionDetails.Tables[2].Rows.Count != 0)
                {
                    AssignDoctors = QuestionDetails.Tables[2];
                }
                else
                {
                    AssignDoctors = QuestionDetails.Tables[2];
                    DataRow row = AssignDoctors.NewRow();
                    row["id"] = 0;
                    row["name"] = "";
                    row["cities"] = "";
                    row["specialities"] = "";
                    row["userid"] = 0;
                    row["locations"] = "";
                    AssignDoctors.Rows.Add(row);
                }
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
                    int[] values = new int[Selectedtags.Count];
                    for (int i = 0; i < Selectedtags.Count; i++)
                    {
                        values[i] = Selectedtags.ToList()[i].tagid;
                    }
                    MultiSelectList makeSelected = new MultiSelectList(tags, "tagid", "tagname", values);
                    ViewBag.tags = makeSelected;
                }
                else
                {
                    tag ObjectTag = new tag();
                    ObjectTag.tagid = 0;
                    ObjectTag.tagname = "";
                    tags.Add(ObjectTag);
                    MultiSelectList makeSelected = new MultiSelectList(tags, "tagid", "tagname", tags);
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
        }
        public JsonResult AssignDoctor(int QuestionId, string AssignDoctorIds)
        {
            string[] ArrayOfID = null;

            questionId = QuestionId;
            AssignDoctors = QuestionManager.getInstance().assignDoctorToQuestion(Convert.ToString(Session["UserId"]), questionId, AssignDoctorIds).Tables[0];

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
                            string Logoimage = Server.MapPath(@"~/Content/image/LogoForMail.png");
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

        public ActionResult QuestionList(string questionsType = "")
        {
            int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.questionlist));
            if (privilege == 1)
            {
                return RedirectToAction("NoPrivilegeError", "Home");
            }
            else
            {
                bool filter = false;
                if (questionsType.Contains("All") || string.IsNullOrEmpty(questionsType))
                {
                    filter = true;
                }
                List<QuestionModel> Qmodel = new List<QuestionModel>();
                Qmodel.Add(new QuestionModel { Filter = filter, AnsweredBy = filter ? 1 : 0, Counts = ConfigurationManager.AppSettings["NumberOfRecoredonQuestionList"].ToString() });
                return View(Qmodel);
            }
        }

        [HttpGet]
        public JsonResult RejectQuestionByQuestionID(string qusetionID)
        {
            SqlConnection conn = null;
            string jsonObj;
            int QusetionID = Convert.ToInt32(qusetionID);
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
                //SMS is stopped for patient
                //string SmsText = ConfigurationManager.AppSettings["OnRemoveQuestionFromListSMS"].ToString();
                // SMS.SendSMS(Convert.ToString(dtUserDetails.Rows[0]["mobileno"]), SmsText);
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

        public ActionResult Report(string cityid = null, string locationid = null, string specialityOrName = null)
        {
            int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.reports));
            if (privilege == 1)
            {
                return RedirectToAction("NoPrivilegeError", "Home");
            }
            else
            {
                DataSet doctorList = null;
                SqlParameter[] param = new SqlParameter[4];
                Report reportData;
                List<Report> ListreportData = new List<Report>();             
                using (conn = SqlHelper.GetSQLConnection())
                {

                    if (!String.IsNullOrEmpty(cityid))
                    {
                        param[0] = new SqlParameter("@cityid", Convert.ToInt32(cityid));
                    }
                    else
                    {
                        param[0] = new SqlParameter("@cityid", System.DBNull.Value);
                    }
                    if (!String.IsNullOrEmpty(locationid))
                    {
                        param[1] = new SqlParameter("@locationid", Convert.ToInt32(locationid));
                    }
                    else
                    {
                        param[1] = new SqlParameter("@locationid", System.DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(specialityOrName))
                    {
                        if (specialityOrName.Contains(' '))
                        {
                            param[2] = new SqlParameter("@specialityOrName", specialityOrName.Replace(' ', '%'));
                        }
                        else
                        {
                            param[2] = new SqlParameter("@specialityOrName", specialityOrName);
                        }
                    }
                    else
                    {
                        param[2] = new SqlParameter("@specialityOrName", System.DBNull.Value);
                    }
                    param[3] = new SqlParameter("@userStatus", UserStatus.Approved);
                    doctorList = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getDoctorListByLoactionNameCitySpeciality", param);
                }
                if ((doctorList != null) && (doctorList.Tables.Count > 0) && doctorList.Tables[0].Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty((Convert.ToString(doctorList.Tables[0].Rows[0]["averageresponsetime"]))))
                    {
                        string avgtime = Convert.ToString((Convert.ToDouble(doctorList.Tables[0].Rows[0]["averageresponsetime"]) / 60).ToString("0.00"));
                        ViewBag.avgresponsetime = avgtime;
                    }
                    DataTable dtDoctors = doctorList.Tables[0];
                    if (dtDoctors != null && dtDoctors.Rows.Count != 0)
                    {
                        for (int i = 0; i < dtDoctors.Rows.Count; i++)
                        {
                            reportData = new Report();
                            reportData.userid = Convert.ToInt32(dtDoctors.Rows[i]["userid"]);
                            reportData.name = dtDoctors.Rows[i]["name"].ToString();
                            reportData.city = dtDoctors.Rows[i]["cities"].ToString();
                            reportData.location = dtDoctors.Rows[i]["locations"].ToString();
                            if (!String.IsNullOrEmpty(dtDoctors.Rows[i]["appointmentcount"].ToString()))
                            {
                                reportData.appointmentbooked = Convert.ToInt32(dtDoctors.Rows[i]["appointmentcount"]);
                            }
                            else
                            {
                                reportData.appointmentbooked = 0;
                            }
                            if (!String.IsNullOrEmpty(dtDoctors.Rows[i]["appointmentbuttonhits"].ToString()))
                            {
                                reportData.appointmentclicked = Convert.ToInt32(dtDoctors.Rows[i]["appointmentbuttonhits"]);
                            }
                            else
                            {
                                reportData.appointmentbooked = 0;
                            }
                            if (dtDoctors.Rows[i]["avgresponsetime"] != null && Convert.ToString(dtDoctors.Rows[i]["avgresponsetime"]) != "")
                                reportData.avgresponsetime = Convert.ToString((Convert.ToDouble(dtDoctors.Rows[i]["avgresponsetime"]) / 60).ToString("0.00"));

                            ListreportData.Add(reportData);
                        }
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Report", ListreportData);
                }
                else
                {
                    return View(ListreportData);
                }
            }
        }

        public SqlConnection conn { get; set; }
        public ActionResult ManageTag()
        {
            int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.managetags));
            if (privilege == 1)
            {
                return RedirectToAction("NoPrivilegeError", "Home");
            }
            else
            return View();
        }
    }

}
