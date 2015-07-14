using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiraiConsultMVC.Controllers
{
    public class PAController : Controller
    {
        // GET: PA
        public ActionResult Index()
        {
            return View("PAHome");
        }
    }
}