using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models.Home;
using System.Configuration;
using MiraiConsultMVC.Models;

namespace MiraiConsultMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
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
    }
}
