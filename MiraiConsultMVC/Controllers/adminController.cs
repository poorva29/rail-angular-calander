using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using System.Data;
using DAL;
using System.Configuration;
using askmirai;
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

            //if (Request.QueryString["questionid"] != null)
            //{
            //    questionId = Convert.ToInt32(Request.QueryString["questionid"].ToString());
            //}
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt32(Session["UserId"].ToString());
            }
            QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyId(questionId, userId, assignQuestion, Convert.ToInt32(QuestionStatus.Approved));
            //if (Request.Params["__EVENTTARGET"] != null && Request.Params["__EVENTTARGET"] != "")
            //{
            //    if (Request.Params["__EVENTTARGET"] == "AssignDoctor")
            //    {
                    //string[] ArrayOfID = null;
                    //string AssignDoctorIds = "6";
                    //questionId = 2355;// Convert.ToInt32(Request.QueryString["questionid"].ToString());
                    //AssignDoctors = QuestionManager.getInstance().assignDoctorToQuestion(questionId, AssignDoctorIds).Tables[0];
                    //if (AssignDoctors != null && AssignDoctors.Rows.Count != 0)
                    //{
                    //    if (!String.IsNullOrEmpty(AssignDoctorIds))
                    //    {
                    //        ArrayOfID = AssignDoctorIds.Split(',');
                    //    }
                    //    for (int i = 0; i < AssignDoctors.Rows.Count; i++)
                    //    {
                    //        if (ArrayOfID.Contains(Convert.ToString(AssignDoctors.Rows[i]["userid"])))
                    //        {
                    //            string msgText = ConfigurationManager.AppSettings["OnDocAssignQuestionSendEmail"].ToString();
                    //            if (AssignDoctors.Rows[i]["lastname"] != System.DBNull.Value && QuestionDetails.Tables[0].Rows[0]["questiontext"] != System.DBNull.Value && AssignDoctors.Rows[i]["mobileno"] != System.DBNull.Value)
                    //            {
                    //                string emailBody = EmailTemplates.GetEmailTemplateOnQuestionAssign(msgText, Convert.ToString(AssignDoctors.Rows[i]["lastname"]), QuestionDetails.Tables[0].Rows[0]["questiontext"].ToString());
                    //                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    //                string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                    //                Mail.SendHTMLMailWithImage(fromEmail, Convert.ToString(AssignDoctors.Rows[i]["email"]), "Mirai Consult - Assigned one question to you", emailBody, Logoimage);
                    //                string SmsText = ConfigurationManager.AppSettings["OnDocAssignQuestionSendSMS"].ToString();
                    //                SMS.SendSMS(Convert.ToString(AssignDoctors.Rows[i]["mobileno"]), SmsText);
                    //            }
                    //        }
                    //    }
                    //}
            //    }
            //}
            //else
            //{
                AssignDoctors = QuestionDetails.Tables[2];
            //}
            //if (!IsPostBack)
            //{
                //DataTable dtTags = UtilityManager.getInstance().getAlltags();
                //Utilities.FillListBox(dtTags, lstOfTags, "N");
                //if (QuestionDetails != null && QuestionDetails.Tables.Count != 0 && QuestionDetails.Tables[1].Rows.Count != 0)
                //{
                //    DataTable tags = QuestionDetails.Tables[1];
                //    for (int i = 0; i < tags.Rows.Count; i++)
                //    {
                //        for (int j = 0; j <= lstOfTags.Items.Count - 1; j++)
                //        {
                //            if (Convert.ToString(tags.Rows[i]["tagid"]) == lstOfTags.Items[j].Value)
                //            {
                //                lstOfTags.Items[j].Selected = true;
                //                break;
                //            }
                //        }
                //    }
                //}
            //}


                List<AssignQuestion> viewmodel = new List<AssignQuestion>();

                viewmodel = AssignDoctors.AsEnumerable().Select(dataRow => new AssignQuestion
                {
                    id = dataRow.Field<int>("id"),
                    name = dataRow.Field<string>("name"),
                    cities = dataRow.Field<string>("cities"),
                    specialities = dataRow.Field<string>("specialities"),
                    userid = dataRow.Field<int>("userid"),
                    locations = dataRow.Field<string>("locations"),
                    questiontext = QuestionDetails.Tables[0].Rows[0]["questiontext"].ToString()
                }).ToList();

            
                

                return View(viewmodel);
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
    }
}
