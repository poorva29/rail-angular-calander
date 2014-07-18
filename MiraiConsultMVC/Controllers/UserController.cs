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
        [HttpGet]
        public ActionResult PatientSignUp()
        {
            _dbAskMiraiDataContext _db = new _dbAskMiraiDataContext();
            ViewBag.Countries = new SelectList(_db.countries, "countryid", "Name");
            ViewBag.State = new SelectList(_db.states, "stateid", "Name");
            ViewBag.City = new SelectList(_db.cities, "cityid", "Name");
            ViewBag.Location = new SelectList(_db.locations, "locationid", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult PatientSignUp(user user)
        {
            if (ModelState.IsValid)
            {
                _dbAskMiraiDataContext _db = new _dbAskMiraiDataContext();
                _db.users.InsertOnSubmit(user);
                _db.SubmitChanges();
            }
            return View();
        }
        [HttpGet]
        public ActionResult DoctorSignUp()
        {
            UtilityManager utilityManager = new UtilityManager();
            _dbAskMiraiDataContext _db = new _dbAskMiraiDataContext();
            ViewBag.Countries = new SelectList(_db.countries, "countryid", "Name");
            ViewBag.Specialities = new SelectList(_db.specialities, "specialityid", "Name");
            ViewBag.Registrationcouncil = new SelectList(_db.registrationcouncils, "regcouncilid", "name");
            return View();
        }
        [HttpPost]
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
        //public ActionResult GetCounsilList(int CountryId)
        //{ 
        //return 
        //}
    }
}
