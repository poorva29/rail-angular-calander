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
namespace MiraiConsultMVC.Controllers
{
    public class HomeController : Controller
    {
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        BasePage BPage = new BasePage();
        public ActionResult Home()
        {
            if(Request.IsAjaxRequest())
            {
              return  PartialView("_AutoCompleteSearch");
            }
            else
            {
                return View();
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
                return View();
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
                msgBody = "<b>Sender Name:</b> " + name + "<br> <b>Sender Email:</b> " + from_address + "<br><b> Message: </b>" + msgBody;
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
            if (Session["locationid"] != null && Session["cityid"] != null)
            {
                findDoctorModel.hdnCityId = Convert.ToInt32(Session["cityid"]) != 0 ? Convert.ToString(Session["cityid"]) : null;
                findDoctorModel.hdnLoactionId = Convert.ToInt32(Session["locationid"]) != 0 ? Convert.ToString(Session["locationid"]) : null;
            }
            else
            {
                findDoctorModel.hdnCityId = null;
                findDoctorModel.hdnLoactionId = null;
            }
            findDoctorModel.hdnDocconnecturl = Convert.ToString(ConfigurationSettings.AppSettings["DocConnectApptUrl"]);
            return View(findDoctorModel);
        }
    }
}
