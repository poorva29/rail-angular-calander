﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models.User;
using MiraiConsultMVC.Models;
using System.Configuration;
using MiraiConsultMVC.Models.Utilities;


namespace MiraiConsultMVC.Controllers
{
    public class UserController : Controller
    {
       
        //
        // GET: /User/
       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login log)
        {
            //Utilities U = new Utilities();
            string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"]; // Please make sure that this username doesn't exist in Patient, Doctor, DoctorAssistant table
            string SuperAdminUserPassword = ConfigurationManager.AppSettings["SuperAdminUserPassword"].ToString();
            string dbpasswd = UtilityManager.Encrypt(log.Password);
            if (log.Email != SuperAdminEmailId && !String.IsNullOrEmpty(log.Email))
            {
                _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
                var isLogin = db.users.FirstOrDefault(x => x.email.Equals(log.Email) && x.password.Equals(dbpasswd));
                if (isLogin != null)
                {
                    if (Convert.ToBoolean(isLogin.isemailverified))
                    {
                        Session["UserFirstName"] = isLogin.firstname;
                        Session["UserLastName"] = isLogin.lastname;
                        Session["UserFullName"] = isLogin.firstname + " " + isLogin.lastname;
                        Session["UserName"] = isLogin.username;
                        Session["UserEmail"] = isLogin.email;
                        Session["UserId"] = isLogin.userid;
                        Session["UserType"] = isLogin.usertype;
                        if (Convert.ToInt32(Session["UserType"]) == 1)
                        {
                            if (Convert.ToInt32(isLogin.status) == 1)
                            {
                                ViewBag.errorMsg = "Dear Doctor, Your account is waiting for approval from MiraiHealth. Please log-in after you receive the activation email.";
                                return View();
                            }
                            else if (Convert.ToInt32(isLogin.status) == 3)
                            {
                                ViewBag.errorMsg = "Dear Doctor, Your account is Rejected.";
                                return View();
                            }
                            return RedirectToAction("ManageDoctors");
                            //redirect to doctor page
                        }
                        else if (Convert.ToInt32(Session["UserType"]) == 2)
                        {
                            return RedirectToAction("ManageDoctors");
                            // redirect to patient page
                        }
                        else if (Convert.ToInt32(Session["UserType"]) == 3)
                        {
                            return RedirectToAction("ManageDoctors");
                            // redirect to assistent page
                        }
                    }
                    else
                    {
                        ViewBag.errorMsg = "Your email is not verified, Please verify your email.";
                        return View();
                    }
                }
                {
                    ViewBag.errorMsg = "Email Id or Password does not match.";
                    return View();
                }
            
            }
            else if (log.Email == SuperAdminEmailId)
            {
                Session["UserFirstName"] = "super";
                Session["UserLastName"] = "admin";
                Session["UserEmail"] = SuperAdminEmailId;
                Session["UserId"] = 9999999;
                Session["UserType"] = 0;
                return RedirectToAction("ManageDoctors");
            }
            else{
                ViewBag.errorMsg= "Email Id or Password does not match";
                  return View();
            }
          
        }

        public ActionResult ManageDoctors()
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
