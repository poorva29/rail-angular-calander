﻿using System;
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
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Reflection;
using System.Data.Linq.Mapping;
using MiraiConsultMVC;



namespace MiraiConsultMVC.Controllers
{
    public class UserController : Controller
    {
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
       
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
                string dbpasswd = Utilities.Encrypt(passwords.currentPassword);
                var userRecord = db.users.FirstOrDefault(x => x.userid.Equals(userID) && x.password.Equals(dbpasswd));

                if (userRecord != null)
                {
                    userRecord.password = Utilities.Encrypt(passwords.newPassword); ;
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
            if (ModelState.IsValid)
            {
                string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"]; // Please make sure that this username doesn't exist in Patient, Doctor, DoctorAssistant table
                string SuperAdminUserPassword = ConfigurationManager.AppSettings["SuperAdminUserPassword"].ToString();
                string dbpasswd = Utilities.Encrypt(log.Password);
                if (log.Email != SuperAdminEmailId && !String.IsNullOrEmpty(log.Email))
                {

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
                else
                {
                    ViewBag.errorMsg = "Email Id or Password does not match";
                    return View();
                }
            }
            return View();
        }

        public ActionResult ManageDoctors(string Registered=null,string Approved=null,string Rejected=null )
        {
            IList<ModelUser> lstdoctors = getAllDoctorDetails();
            if(Request.IsAjaxRequest())
            {
                Boolean IsRegistered = false;
                Boolean IsApproved = false;
                Boolean IsRejected = false;
                IList<ModelUser> lstFilterDoctors;
                if (!String.IsNullOrEmpty(Registered))
                {
                    IsRegistered = true;
                }
                if (!String.IsNullOrEmpty(Approved))
                {
                    IsApproved = true;
                }
                if (!String.IsNullOrEmpty(Rejected))
                {
                    IsRejected = true;
                }
                if (IsRegistered && IsApproved && IsRejected)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(1) || d.Status.Equals(2) || d.Status.Equals(3)).ToList();
                }
                else if (IsRegistered && IsApproved)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(1) || d.Status.Equals(2)).ToList();
                }
                else if (IsRegistered && IsRejected)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(1) || d.Status.Equals(3)).ToList();
                }
                else if (IsApproved && IsRejected)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(2) || d.Status.Equals(3)).ToList();
                }
                else if (IsRegistered)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(1)).ToList();
                }
                else if (IsRejected)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(3)).ToList();
                }
                else if (IsApproved)
                {
                    lstFilterDoctors = lstdoctors.Where(d => d.Status.Equals(2)).ToList();
                }
                else
                {
                    lstFilterDoctors = lstdoctors;
                }
                return PartialView("_DoctorDetails", lstFilterDoctors);
            }
            lstdoctors = lstdoctors.Where(d => d.Status.Equals(1)).ToList();
            return View(lstdoctors);
        }

        public JsonResult ApproveDoctor(string doctorid, string DoctorMobile, string DoctorName, string DoctorEmail)
        {
            string smsText = ConfigurationManager.AppSettings["DocApproveNotificationTextMsg"].ToString();
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
            string subject = "Mirai Consult - Your registration request to Mirai Consult has been approved";
            string body = EmailTemplates.GetTemplateOfApprovalNotificationEmailToDoc(DoctorName);
            int DoctorID = Convert.ToInt32(doctorid);
            object jsonObj ;
            int statusApp = (int)UserStatus.Approved;
             _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            var UserRecord = db.users.FirstOrDefault(x => x.userid.Equals(DoctorID));
            if (UserRecord != null)
            {
                    UserRecord.status = statusApp; 
                    db.SubmitChanges();
                    string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                    Mail.SendHTMLMailWithImage(fromEmail, DoctorEmail, subject, body, Logoimage);
                    SMS.SendSMS(Convert.ToString(DoctorMobile), smsText);
                    jsonObj = "Doctor has been Approved successfully.";
                   
             }
             else
             {
                 jsonObj = "Unable to change the status.";
             }
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RejectDoctorByDoctorId(string doctorID, string DoctorMobile, string DoctorEmail)
        {
            
            string smsText = ConfigurationManager.AppSettings["DocRejectNotificationTextMsg"].ToString();
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
            string subject = "Mirai Consult - your registration request has been rejected.";
            string body = EmailTemplates.EmailNotificationTempleteForRejectedDoctor();
            int DoctorID = Convert.ToInt32(doctorID);
            int statusRejected = (int)UserStatus.Rejected;
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            object jsonObj;
            var UserRecord = db.users.FirstOrDefault(x => x.userid.Equals(DoctorID));
            if (UserRecord != null)
            {
                    UserRecord.status = statusRejected; 
                    db.SubmitChanges();
                    string Logoimage = Server.MapPath("..\\Resources\\image\\LogoForMail.png");
                    Mail.SendHTMLMailWithImage(fromEmail, DoctorEmail, subject, body, Logoimage);
                    SMS.SendSMS(Convert.ToString(DoctorMobile), smsText);
                    jsonObj = "Doctor has been Rejected successfully.";
            }
            else
            {
                   jsonObj = "Unable to change the status";
            }
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FilterManageDoctors()
        {
            IList<ModelUser> lstdoctors = getAllDoctorDetails();
            return PartialView(lstdoctors);
        }

        public IList<ModelUser> getAllDoctorDetails()
        {
            IList<ModelUser> lstdoctors = new List<ModelUser>();
            DataSet dsDoctorDetails = null;
            SqlConnection conn = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsDoctorDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_get_alldoctorsdetails");
            }
            lstdoctors = populateDoctorDetails(dsDoctorDetails);
            return lstdoctors;
        }
        

        private static IList<ModelUser> populateDoctorDetails(DataSet dsDoctorDetails)
        {
            IList<ModelUser> lstdoctors = new List<ModelUser>();
            ModelUser doctor;
            int doctorid;
            DataView dvdocspecialities, dvdoctorlocations, dvdoctorqualifications, dvdoctorsdetails;
            DataRow[] datarows;
            if (dsDoctorDetails != null && dsDoctorDetails.Tables.Count > 0 && dsDoctorDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsDoctorDetails.Tables[0].Rows)
                {
                    doctor = new ModelUser();

                    doctorid = Convert.ToInt32(dr["userid"]);
                    doctor.UserId = doctorid;
                    doctor.FirstName = Convert.ToString(dr["firstname"]);
                    doctor.LastName = Convert.ToString(dr["lastname"]);
                    doctor.Email = Convert.ToString(dr["email"]);
                    doctor.MobileNo = Convert.ToString(dr["mobileno"]);
                    doctor.Gender = Convert.ToInt32(dr["gender"]);
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["dateofbirth"])))
                    {
                        doctor.DateOfBirth = Convert.ToDateTime(dr["dateofbirth"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["photopath"])))
                    {
                        doctor.Image = Convert.ToString(dr["photopath"]);
                    }
                    doctor.UserName = Convert.ToString(dr["username"]);
                    doctor.Password = Convert.ToString(dr["password"]);
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["isemailverified"])))
                    {
                        doctor.IsEmailVerified = Convert.ToBoolean(dr["isemailverified"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["status"])))
                    {
                        doctor.Status = Convert.ToInt32(dr["status"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["registrationdate"])))
                    {
                        doctor.RegistrationDate = Convert.ToDateTime(dr["registrationdate"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["registrationnumber"])))
                    {
                        doctor.RegistrationNumber = Convert.ToString(dr["registrationnumber"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["registrationcouncil"])))
                    {
                        doctor.RegistrationCouncil = Convert.ToInt32(dr["registrationcouncil"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["aboutme"])))
                    {
                        doctor.AboutMe = Convert.ToString(dr["aboutme"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["countryid"])))
                    {
                        doctor.CountryId = Convert.ToInt32(dr["countryid"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["photourl"])))
                    {
                        doctor.PhotoUrl = Convert.ToString(dr["photourl"]);
                    }
                    if (dsDoctorDetails.Tables.Count == 5)
                    {
                        dvdocspecialities = new DataView(dsDoctorDetails.Tables[1]);
                        dvdoctorlocations = new DataView(dsDoctorDetails.Tables[2]);
                        dvdoctorqualifications = new DataView(dsDoctorDetails.Tables[3]);
                        dvdoctorsdetails = new DataView(dsDoctorDetails.Tables[4]);
                        string expression = "userid =" + doctorid;
                        string sortOrder = "";
                        datarows = dvdocspecialities.Table.Select(expression, sortOrder);
                        foreach (DataRow dr1 in datarows)
                        {
                            DoctorSpecialities doctorspeciality = new DoctorSpecialities();
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["specialityid"])))
                            {
                                doctorspeciality.SpecialityId = Convert.ToInt32(dr1["specialityid"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["speciality_name"])))
                            {
                                doctorspeciality.Speciality = Convert.ToString(dr1["speciality_name"]);

                            }
                            doctor.specialities.Add(doctorspeciality);
                        }
                        datarows = dvdoctorlocations.Table.Select(expression, sortOrder);
                        bool isloc_already_added = false;

                        foreach (DataRow drlocation in datarows)
                        {
                            DoctorLocations doctorlocation = new DoctorLocations();
                            isloc_already_added = doctor.locations.Any(e => e.DoctorLocationId == Convert.ToInt32(drlocation["doctorlocationid"]));
                            if (!isloc_already_added)
                            {
                                doctorlocation.DoctorLocationId = Convert.ToInt32(drlocation["doctorlocationid"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["countryid"])))
                                {
                                    doctorlocation.CountryId = Convert.ToInt32(drlocation["countryid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["country_name"])))
                                {
                                    doctorlocation.Country = Convert.ToString(drlocation["country_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["stateid"])))
                                {
                                    doctorlocation.StateId = Convert.ToInt32(drlocation["stateid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["state_name"])))
                                {
                                    doctorlocation.State = Convert.ToString(drlocation["state_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["cityid"])))
                                {
                                    doctorlocation.CityId = Convert.ToInt32(drlocation["cityid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["city_name"])))
                                {
                                    doctorlocation.City = Convert.ToString(drlocation["city_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["locationid"])))
                                {
                                    doctorlocation.LocationId = Convert.ToInt32(drlocation["locationid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["clinicname"])))
                                {
                                    doctorlocation.ClinicName = Convert.ToString(drlocation["clinicname"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["location_name"])))
                                {
                                    doctorlocation.Location = Convert.ToString(drlocation["location_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["telephone"])))
                                {
                                    doctorlocation.Telephone = Convert.ToString(drlocation["telephone"]);
                                }
                                if (!String.IsNullOrEmpty(drlocation["address"].ToString()))
                                {
                                    doctorlocation.Address = drlocation["address"].ToString();
                                }
                                doctor.locations.Add(doctorlocation);
                            }
                        }
                        datarows = dvdoctorqualifications.Table.Select(expression, sortOrder);
                        foreach (DataRow dr1 in datarows)
                        {
                            doctorqualifications doctorqualification = new doctorqualifications();
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["degreeid"])))
                            {
                                doctorqualification.DegreeId = Convert.ToInt32(dr1["degreeid"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["degree_name"])))
                            {
                                doctorqualification.Degree = Convert.ToString(dr1["degree_name"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["university"])))
                            {
                                doctorqualification.University = Convert.ToString(dr1["university"]);

                            }
                            doctor.qualification.Add(doctorqualification);
                        }
                        datarows = dvdoctorsdetails.Table.Select(expression, sortOrder);
                        foreach (DataRow drdoctorsdetails in datarows)
                        {
                            doctordetails doctordetails = new doctordetails();
                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["docdetailsid"])))
                            {
                                doctordetails.DocDetailsId = Convert.ToInt32(drdoctorsdetails["docdetailsid"]);
                            }

                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["userid"])))
                            {
                                doctordetails.UserId = Convert.ToInt32(drdoctorsdetails["userid"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["certification"])))
                            {
                                doctordetails.Certification = Convert.ToString(drdoctorsdetails["certification"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["society"])))
                            {
                                doctordetails.Society = Convert.ToString(drdoctorsdetails["society"]);
                            }
                            doctor.details.Add(doctordetails);
                        }
                        lstdoctors.Add(doctor);
                    }
                }
            }
            return lstdoctors;
        }

        public DataSet get_AuthenticateData(string username, string password)
        {
            DataSet dsUserDetails = null;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@username", username);
            param[1] = new SqlParameter("@password", password);
            SqlConnection conn = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsUserDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_AuthenticateData", param);
            }

            if (dsUserDetails != null && dsUserDetails.Tables.Count != 0)
                return dsUserDetails;
            else
                return null;
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

          [HttpPost]
        public ActionResult ForgotPassword(string name)
        {
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            var UserRecord = db.users.FirstOrDefault(x => x.email.Equals(name));
            if (UserRecord != null)
            {
                string emailVerficationURL = Convert.ToString(ConfigurationManager.AppSettings["ResetPasswordLink"]);
                string emailBody = EmailTemplates.SendResetPasswordNotificationEmail(UserRecord.userid.ToString(), UserRecord.firstname+" "+UserRecord.lastname, emailVerficationURL);
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                Mail.SendHTMLMailWithImage(fromEmail, name, "Mirai Consult - reset your password", emailBody, Logoimage);
                ViewBag.success="true";
                ViewBag.Msg = "Email has been sent to your email address. After clicking on the link in the email, you can reset your password.";
            }
            else{
                  ViewBag.success="false";
                  ViewBag.Msg = "Invalid email address or you have not verified your email address.";
            }

            return View(); //return some view to the user
        }

        public ActionResult ResetPassword(string id)
        {
            TempData["userid"] = Utilities.Decrypt(HttpUtility.UrlDecode(id.ToString()).Replace(" ", "+"));
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword passwords)
        {
            if (ModelState.IsValid)
            {
                _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
                int userID = Convert.ToInt32(TempData["userid"].ToString());

                string dbpasswd = Utilities.Encrypt(passwords.Password);
                var userRecord = db.users.FirstOrDefault(x => x.userid.Equals(userID));
                if (userRecord != null)
                {
                    userRecord.password = Utilities.Encrypt(passwords.Password); ;

                    db.SubmitChanges();
                    ViewBag.errorMsg = "Password has been Reset successfully.";
                   
                }
            }
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
