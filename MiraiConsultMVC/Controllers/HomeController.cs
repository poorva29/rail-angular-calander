using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models.Home;
using System.Configuration;
using MiraiConsultMVC.Models;
using MiraiConsultMVC;
using Newtonsoft.Json;
using System.Data;
using DAL;
using System.Text.RegularExpressions;
using log4net;
using TagCloud;
using Services;

namespace MiraiConsultMVC.Controllers
{
    public class HomeController : Controller
    {
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        BasePage BPage = new BasePage();
        public ActionResult Home()
        {
            ViewBag.AskmiraiUrl = Convert.ToString(ConfigurationSettings.AppSettings["askMiraiLink"]);
            if(Request.IsAjaxRequest())
            {
              return  PartialView("_AutoCompleteSearch");
            }
            else
            {
                UserService US = new UserService();
                Tag t = new Tag();
                decimal tagPercent = 0;
                List<Tag> ListTags = new List<Tag>();

                ListTags = UtilityManager.getInstance().get_allTagsWithCountOfAnsweredQuestions().Tables[0].AsEnumerable().Select(dataRow => new Tag
                {
                    Text = dataRow.Field<string>("tagname"),
                    NavigateUrl = "/topics/" + dataRow.Field<string>("tagname").Replace(' ', '-'),
                    TagWeight = Convert.ToInt32(dataRow.Field<string>("counts")),
                    ToolTip = dataRow.Field<string>("tagname")

                }).ToList();
                ListTags = ListTags.Take(20).OrderBy(x => x.Text).ToList();
                decimal totalTagWeight = ListTags.Sum(x => x.TagWeight);
                foreach (Tag tg in ListTags)
                {
                    switch (tg.TagWeight)
                    {
                        case 10:
                        case 9:
                        case 8:
                        case 7:
                        case 6:
                        case 5:
                            tg.CssClass = "tag5";
                            break;
                        case 4:
                            tg.CssClass = "tag4";
                            break;
                        case 3:
                            tg.CssClass = "tag3";
                            break;
                        case 2:
                            tg.CssClass = "tag2";
                            break;
                        case 1:
                            tg.CssClass = "tag1";
                            break;
                        default:
                            tg.CssClass = "tag0";
                            break;
                    }
                    #region Commented Code
                    //tagPercent = (Convert.ToDecimal(tg.TagWeight) * 100 / totalTagWeight);
                    //if (tgtagPercent >= 90)
                    //{
                    //    tg.CssClass = "tag5";
                    //}
                    //else if (tagPercent >= 70)
                    //{
                    //    tg.CssClass = "tag4";
                    //}
                    //else if (tagPercent >= 40)
                    //{
                    //    tg.CssClass = "tag3";
                    //}
                    //else if (tagPercent >= 20)
                    //{
                    //    tg.CssClass = "tag2";
                    //}
                    //else if (tagPercent >= 3)
                    //{
                    //    tg.CssClass = "tag1";
                    //}
                    //else if (tagPercent >= 2)
                    //{
                    //    tg.CssClass = "tag1";
                    //}
                    //else if (tagPercent >= 1)
                    //{
                    //    tg.CssClass = "tag1";
                    //}
                    //else
                    //{
                    //    tg.CssClass = "tag0";
                    //}
                    #endregion
                }
                return View(ListTags);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsOfUse()
        {
            return View();
        }

        public ActionResult HowWeWorks()
        {
            return View();
        }

        public ActionResult NoPrivilegeError()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Error(string name)
        {
            bool retValue = false;
            ILog logfile = LogManager.GetLogger(typeof(HomeController));
            string  userId = Convert.ToString(Session["UserId"]);
            string userEmailId = Convert.ToString(Session["UserEmailId"]);
            Exception ex = (Exception)TempData["Exception"];
            if (ex is System.Web.HttpRequestValidationException)
            {
                ViewBag.Msg = "HTML / XML tags not allowed in input.<BR> We have detected an XML / HTML tag in one of the text input fields. These tags are not allowed in the input.";
                string exceptionDetails = Regex.Replace(ex.ToString(), "<.*?>", string.Empty);
                string strError = "<b>Error Has Been Caught in Page_Error event</b><hr><br/>" +
                              "<b>Current Date: </b>" + System.DateTime.Now + "<br/>" +
                                        "<b>UserID: </b>" + userId +
                                        "<br><b>User EmailID: </b>" + userEmailId +
                                        "<br><b>Error Message: </b>" + exceptionDetails +
                                        "<br><b>Stack Trace:</b><br/>" + ex.StackTrace.ToString() + "<br/>";
                TempData["ExceptionDetails"] = strError;
                ViewBag.success = "false";
            }
            if (ex != null)
            {
                string exceptionDetails = Regex.Replace(ex.Message.ToString(), "<.*?>", string.Empty);
                string strError = "<b>Error Has Been Caught in Page_Error event</b><hr><br/>" +
                            "<b>Current Date: </b>" + System.DateTime.Now + "<br/>" +
                            "<b>UserID: </b>" + userId +
                            "<br><b>User EmailID: </b>" + userEmailId +
                            "<br><b>Error Message: </b>" + exceptionDetails +
                            "<br><b>Stack Trace:</b><br/>" + ex.StackTrace.ToString() + "<br/>";
                TempData["ExceptionDetails"] = strError;
                ViewBag.success = "false";
                logfile.Error("\n\n******************* USER VISIBLE FATAL ERROR **************\n" +
                    "UserID: " + userId + "\nMessage: " + ex.Message + "\n", ex);
            }
            string ErrorDescription = name;
            string EmailID = ConfigurationManager.AppSettings["ErrorEmailID"].ToString();
            string email_Content = EmailTemplates.GetEmailTemplateToSendError(ErrorDescription, Convert.ToString(TempData["ExceptionDetails"]));
            retValue= Mail.SendHTMLMail("", EmailID, "Mirai Consult - Error", email_Content);
            if (retValue)
            {
                ViewBag.success = "true";
                ViewBag.Msg = "Error details has been sent successfully.";
            }
            else
            {
                ViewBag.success = "false";
                ViewBag.Msg = "Unable to send error details, Please try again.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                string from_address = null;
                bool sent_mail = false;
                string name;
                string msgBody = contact.message;
                string subject = "Askmirai Contact Us";
                string emailId = Convert.ToString(ConfigurationManager.AppSettings["ContactUSEmail"].ToString().Trim());
                if (Session["UserId"] != null && Session["UserEmail"] != null)
                {
                    from_address = Convert.ToString(Session["UserEmail"]);
                    name = Convert.ToString(Session["UserFullName"]);
                }
                else
                {
                    from_address = contact.Email;
                    name = contact.Name;
                }
                msgBody = "<b>Sender Name:</b> " + Server.HtmlEncode(name) + "<br> <b>Sender Email:</b> " + from_address + "<br><b> Message: </b>" + Server.HtmlEncode(msgBody);
                sent_mail = Mail.SendHTMLMail(from_address, emailId, subject, msgBody, null);
                if (!sent_mail)
                {
                    ViewBag.Message = "Failed to send email.";
                }
                else
                {
                    ViewBag.Message = "Thank you for contacting us.";
                    ModelState.Clear();
                }
            }
            return View();
        }

        public string AutoComplete(string searchTerm)
        {
            DataSet dsQuestions = QuestionManager.getInstance().searchQuestion(searchTerm, Convert.ToInt32(QuestionStatus.Approved));
            return JsonConvert.SerializeObject(dsQuestions.Tables[0]);
        }

        [HttpGet]
        public ActionResult FindDoctors(FindDoctorModel findDoctorModel)
        {
            if (Session["UserId"] != null && Session["UserType"] != null)
                findDoctorModel.HiddenUserType = Convert.ToString(Session["UserType"]);
            else
                findDoctorModel.HiddenUserType = "-1";
            if (Session["cityid"] != null)
            {
                findDoctorModel.hdnCityId = Convert.ToInt32(Session["cityid"]) != 0 ? Convert.ToString(Session["cityid"]) : null;
                if (Session["locationid"] != null)
                {
                    findDoctorModel.hdnLoactionId = Convert.ToInt32(Session["locationid"]) != 0 ? Convert.ToString(Session["locationid"]) : null;
                }
                else
                {
                    findDoctorModel.hdnLoactionId = null;
                }
            }
            else
            {
                findDoctorModel.hdnCityId = null;
            }
            findDoctorModel.hdnDocconnecturl = Convert.ToString(ConfigurationSettings.AppSettings["DocConnectApptUrl"]);
            return View(findDoctorModel);
        }

        public ActionResult topics()
        {
            UserService US = new UserService();
            Tag t = new Tag();
            decimal tagPercent = 0;
            List<Tag> ListTags = new List<Tag>();

            ListTags = UtilityManager.getInstance().get_allTagsWithCountOfAnsweredQuestions().Tables[0].AsEnumerable().Select(dataRow => new Tag
            {
                Text = dataRow.Field<string>("tagname"),
                NavigateUrl = "/topics/" + dataRow.Field<string>("tagname").Replace(' ','-'),
                TagWeight = Convert.ToInt32(dataRow.Field<string>("counts")),
                ToolTip = dataRow.Field<string>("tagname")
                
               
            }).OrderBy(x => x.Text).ToList();
            decimal totalTagWeight = ListTags.Sum(x => x.TagWeight);
            foreach (Tag tg in ListTags)
            {
                switch (tg.TagWeight)
                {
                    case 10:
                    case 9:
                    case 8:
                    case 7:
                    case 6:
                    case 5:
                        tg.CssClass = "tag5";
                        break;
                    case 4:
                        tg.CssClass = "tag4";
                        break;
                    case 3:
                        tg.CssClass = "tag3";
                        break;
                    case 2:
                        tg.CssClass = "tag2";
                        break;
                    case 1:
                        tg.CssClass = "tag1";
                        break;
                    default:
                        tg.CssClass = "tag0";
                        break;
                }
                //tagPercent = (Convert.ToDecimal(tg.TagWeight) * 100 / totalTagWeight);
                //if (tgtagPercent >= 90)
                //{
                //    tg.CssClass = "tag5";
                //}
                //else if (tagPercent >= 70)
                //{
                //    tg.CssClass = "tag4";
                //}
                //else if (tagPercent >= 40)
                //{
                //    tg.CssClass = "tag3";
                //}
                //else if (tagPercent >= 20)
                //{
                //    tg.CssClass = "tag2";
                //}
                //else if (tagPercent >= 3)
                //{
                //    tg.CssClass = "tag1";
                //}
                //else if (tagPercent >= 2)
                //{
                //    tg.CssClass = "tag1";
                //}
                //else if (tagPercent >= 1)
                //{
                //    tg.CssClass = "tag1";
                //}
                //else
                //{
                //    tg.CssClass = "tag0";
                //}
            }
            return View(ListTags);
        }

        public ActionResult topicdetails(string tag)
        {
            AnswerModel ansModel;
            IList<QuestionModel> lstQuestions = new List<QuestionModel>();
            ViewBag.tag = tag;
            var questionList = db.get_AllQuestionsByTagSEO(tag, Convert.ToInt32(QuestionStatus.Approved)).ToList();
            if (questionList != null && questionList.Count() > 0)
            {
                foreach (var item in questionList)
                {
                    ansModel = new AnswerModel();
                    QuestionModel qModel = new QuestionModel();
                    qModel.answers = new List<AnswerModel>();
                    qModel.QuestionId = Convert.ToInt32(item.questionid);
                    qModel.QuestionText = item.questiontext;
                    qModel.answerreplyedby = item.doctorname;
                    qModel.DocImg = item.docImageUrl + item.docImage;
                    qModel.Counts = item.counts;
                    ansModel.AnswerText = item.answertext;
                    ansModel.AnswerImage = item.ansImage;
                    qModel.answers.Add(ansModel);
                    lstQuestions.Add(qModel);
                }
            }
            //return Json(lstQuestions, JsonRequestBehavior.AllowGet);
            return View(lstQuestions);
        }

    }
}
