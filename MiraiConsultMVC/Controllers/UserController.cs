using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models.User;
using MiraiConsultMVC.Models;
using System.Configuration;
using System.Data;
using DAL;
using Newtonsoft.Json;
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

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Changepassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Changepassword(ChangePassword passwords)
        {
            if (ModelState.IsValid)
            {
                _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
                int userID = Convert.ToInt32(Session["UserId"]);
                string dbpasswd = MiraiConsultMVC.Models.Utilities.UtilityManager.Encrypt(passwords.currentPassword);
                var userRecord = db.users.FirstOrDefault(x => x.userid.Equals(userID) && x.password.Equals(dbpasswd));

                if (userRecord != null)
                {
                    userRecord.password = MiraiConsultMVC.Models.Utilities.UtilityManager.Encrypt(passwords.newPassword); ;
                    db.SubmitChanges();
                    ViewBag.errorMsg = "Password has been changed successfully.";
                    return View();
                }
                else
                {
                    ViewBag.errorMsg = "Please enter valid current password.";
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login log)
        {
            //Utilities U = new Utilities();
            string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"]; // Please make sure that this username doesn't exist in Patient, Doctor, DoctorAssistant table
            string SuperAdminUserPassword = ConfigurationManager.AppSettings["SuperAdminUserPassword"].ToString();
            string dbpasswd = MiraiConsultMVC.Models.Utilities.UtilityManager.Encrypt(log.Password);
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
                        if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Doctor))
                        {
                            if (Convert.ToInt32(isLogin.status) == Convert.ToInt32(UserStatus.Pending))
                            {
                                ViewBag.errorMsg = "Dear Doctor, Your account is waiting for approval from MiraiHealth. Please log-in after you receive the activation email.";
                                return View();
                            }
                            else if (Convert.ToInt32(isLogin.status) == Convert.ToInt32(UserStatus.Registered))
                            {
                                ViewBag.errorMsg = "Dear Doctor, Your account is Rejected.";
                                return View();
                            }
                            return RedirectToAction("ManageDoctors");
                            //redirect to doctor page
                        }
                        else if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Patient))
                        {
                            return RedirectToAction("ManageDoctors");
                            // redirect to patient page
                        }
                        else if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Assistent))
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
            MiraiConsultMVC.Models.Utilities.UtilityManager utilityManager = new MiraiConsultMVC.Models.Utilities.UtilityManager();
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
        public string AutoComplete(string term)
        {
            DataSet dsQuestions = QuestionManager.getInstance().searchQuestion(term, Convert.ToInt32(QuestionStatus.Approved));
            return JsonConvert.SerializeObject(dsQuestions.Tables[0]);
        }
    }
}
