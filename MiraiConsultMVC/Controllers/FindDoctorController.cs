using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using System.Configuration;

namespace MiraiConsultMVC.Controllers
{
    public class FindDoctorController : Controller
    {
        //
        // GET: /Find/

        public ActionResult Index()
        {
            return View();
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
