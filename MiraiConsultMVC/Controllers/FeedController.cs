using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiraiConsultMVC.Controllers
{
    public class FeedController : Controller
    {
        //
        // GET: /Feed/

        public ActionResult Feed()
        {
            return View();
        }
        public ActionResult DoctorFeed()
        {
            return View();
        }

    }
}
