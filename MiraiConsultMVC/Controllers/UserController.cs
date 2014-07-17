using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;

namespace MiraiConsultMVC.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PatientSignUp()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult PatientSignUp(user user)
        {
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            db.users.InsertOnSubmit(user);
            db.SubmitChanges();
            return View();
        }
        public ActionResult DoctorSignUp()
        {
            UtilityManager utilityManager = new UtilityManager();

            

            User user = new Models.User();
            user.Countries = utilityManager.getAllCountries();
            
            return View(user);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DoctorSignUp(user user)
        {
            if (ModelState.IsValid)
            {
                _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
                db.users.InsertOnSubmit(user);
                db.SubmitChanges();
            }
            return View();
        }
    }
}
