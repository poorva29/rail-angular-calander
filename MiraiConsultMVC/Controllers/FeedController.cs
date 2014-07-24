using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using MiraiConsultMVC.Models;
namespace MiraiConsultMVC.Controllers
{
    public class FeedController : Controller
    {
        //
        // GET: /Feed/
        public String AskmiraiUrl="";
        public String FacebookAppKey = "";

        public ActionResult Feed(Feed feed)
        {
            if (Session["UserId"] != null && Session["UserType"] != null)
            {
               feed.HdnUserID  = Convert.ToString(Session["UserId"]);
               feed.HdnRecordSize= ConfigurationManager.AppSettings["NumberOfRecored"].ToString();
               feed.HiddenUserType = Convert.ToString(Session["UserType"]);
               feed.hdnDocconnecturl = Convert.ToString(ConfigurationSettings.AppSettings["DocConnectApptUrl"]);
            }
            AskmiraiUrl =  Convert.ToString(ConfigurationSettings.AppSettings["askMiraiLink"]);
            FacebookAppKey = Convert.ToString(ConfigurationSettings.AppSettings["FacebookAppKey"]);
            return View(feed);
        }

        public ActionResult DoctorFeed()
        {
            return View();
        }

    }
}
