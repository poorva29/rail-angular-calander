using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models.User;
using MiraiConsultMVC.Models;
using System.Configuration;
using DAL;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MiraiConsultMVC;
using System.IO;
using Model;

namespace MiraiConsultMVC.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        BasePage BPage = new BasePage();
        public ActionResult Login()
        {
            Login log = new Login();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            if (Request.Cookies["Consult_UName"] != null)
                log.Email = Request.Cookies["Consult_UName"].Value;
            if (Request.Cookies["Consult_PWD"] != null)
                log.Password = Request.Cookies["Consult_PWD"].Value;
            if (Request.Cookies["Consult_UName"] != null && Request.Cookies["Consult_PWD"] != null)
                log.RememberMe = true;
            return View(log);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Changepassword()
        {
            BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.changepassword));
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
                    ViewBag.success = "Password has been changed successfully.";
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
            if (ModelState.IsValid)
            {
                string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"]; // Please make sure that this username doesn't exist in Patient, Doctor, DoctorAssistant table
                string SuperAdminUserPassword = ConfigurationManager.AppSettings["SuperAdminUserPassword"].ToString();
                string dbpasswd = Utilities.Encrypt(log.Password);
                int userType;
                User user;
                if (log.Email != SuperAdminEmailId && !String.IsNullOrEmpty(log.Email))
                {
                    var isLogin = db.users.FirstOrDefault(x => x.email.Equals(log.Email) && x.password.Equals(dbpasswd));
                    if (isLogin != null)
                    {
                        if (Convert.ToBoolean(isLogin.isemailverified))
                        {
                            user = new User();
                            Session["UserFirstName"] = isLogin.firstname;
                            Session["UserLastName"] = isLogin.lastname;
                            Session["UserFullName"] = isLogin.firstname + " " + isLogin.lastname;
                            Session["UserName"] = isLogin.username;
                            Session["UserEmail"] = isLogin.email;
                            Session["UserId"] = isLogin.userid;
                            Session["UserType"] = isLogin.usertype;
                            Session["locationid"] = isLogin.locationid;
                            Session["cityid"] = isLogin.cityid;
                            userType = Convert.ToInt32(user.UserType);
                            setUserPrivilegesBasedOnUsertype(userType);
                            RememberMe(log.RememberMe, log.Email, dbpasswd);
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

                                 Session["UnQuestionCount"] = showUnansweredQuestionCount();
                                 return RedirectToAction("DoctorQuestionList", "Questions");
                                //redirect to doctor page
                            }
                            else if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Patient))
                            {
                                return RedirectToAction("feed", "feed");
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
                    Session["UserFullName"] = "Super Admin";
                    Session["UserEmail"] = SuperAdminEmailId;
                    Session["UserId"] = 9999999;
                    Session["UserType"] = 0;

                    setUserPrivilegesBasedOnUsertype(0);

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
        protected void RememberMe(bool rememberMe,string email, string password)
        {
            if (rememberMe == true)
            {
                Response.Cookies["Consult_UName"].Value = email;
                Response.Cookies["Consult_PWD"].Value = password;
                Response.Cookies["Consult_UName"].Expires = DateTime.Now.AddMonths(2);
                Response.Cookies["Consult_PWD"].Expires = DateTime.Now.AddMonths(2);
            }
            else
            {
                Response.Cookies["Consult_UName"].Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies["Consult_PWD"].Expires = DateTime.Now.AddMonths(-1);
            }
        }
        
        public ActionResult ManageDoctors(string Registered=null,string Approved=null,string Rejected=null )
        {
            BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.manageDoctor));
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
                            DoctorSpeciality doctorspeciality = new DoctorSpeciality();
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

        public ActionResult Verifyemail(string id, string userType)
        {
            if (id != null)
            {
                bool isEmailVerify = false;
                if (Request.QueryString["isemailverify"] == null)
                {
                    int isLinkActivate = 0;
                    string email = "";
                    int userID = Convert.ToInt32(Utilities.Decrypt(HttpUtility.UrlDecode(id.ToString()).Replace(" ", "+")));
                    isLinkActivate = UtilityManager.getInstance().ActivateEmail(userID, isEmailVerify, email, out isLinkActivate);
                    if (isLinkActivate == 0)
                    {
                         ViewBag.Message= "Sorry, this link has already been used.";
                        //, please contact the system administrator to reissue new link to activate your account
                    }
                    else
                    {
                        if (isLinkActivate == 1)
                        {
                            if (userType == "Doctor")
                            {
                               ViewBag.Message = "Thank you for activating your account. Your request will be verified by an administrator and you will be notified once your account is ready for use.";
                            }
                            if (userType == "Patient")
                            {
                                ViewBag.Message = "Thank you for activating your account. Your email has been verified. Now you can login to the system with your credentials";
                            }
                        }
                    }
                }
                else
                {
                    int isLinkActivate = 0;
                    int userID = Convert.ToInt32(Utilities.Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"].ToString()).Replace(" ", "+")));
                    isEmailVerify = Convert.ToBoolean(Request.QueryString["isemailverify"]);
                    string emailid = Utilities.Decrypt(HttpUtility.UrlDecode(Request.QueryString["email"].ToString()).Replace(" ", "+"));
                    isLinkActivate = UtilityManager.getInstance().ActivateEmail(userID, isEmailVerify, emailid, out isLinkActivate);
                    if (isLinkActivate == 1)
                    {
                        ViewBag.Message = "Thank you for email verification. Now you can receive email notifications from MiraiConsult";
                        Session["IsEmailVerified"] = isEmailVerify;
                    }
                    else
                    {
                        ViewBag.Message = "Sorry, this link has already been used.";
                    }
                }
            }
            return View();
        }

          [HttpPost]
        public ActionResult ForgotPassword(Login log )
        {
            string name = log.Email;
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            var UserRecord = db.users.FirstOrDefault(x => x.email.Equals(name));
            if (UserRecord != null)
            {
                    string emailVerficationURL = Convert.ToString(ConfigurationManager.AppSettings["ResetPasswordLink"]);
                    string emailBody = EmailTemplates.SendResetPasswordNotificationEmail(UserRecord.userid.ToString(), UserRecord.firstname + " " + UserRecord.lastname, emailVerficationURL);
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                    Mail.SendHTMLMailWithImage(fromEmail, name, "Mirai Consult - reset your password", emailBody, Logoimage);
                    ViewBag.success = "true";
                    ViewBag.Msg = "Email has been sent to your email address. After clicking on the link in the email, you can reset your password.";
            }
            else
            {
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
          ModelUser modelUser = new ModelUser();
          var countryList = poupulateCountry();
          modelUser.Countries = new SelectList(countryList, "countryid", "name");
          return View(modelUser);
        }
        [HttpPost]
        public ActionResult PatientSignUp(ModelUser values)
        {
            if (ModelState.IsValid)
            {
                values.Password = Utilities.Encrypt(values.Password);
                values.RegistrationDate = DateTime.Now;
                values.UserType = Convert.ToInt32(UserType.Patient);
                values.UserId = Convert.ToInt32(Session["UserId"]);
                values.Status = Convert.ToInt32(UserStatus.Pending);
                values.IsEmailVerified = false;
                if (values.DateOfBirth != null)
                    values.DateOfBirth = DateTime.Parse(Convert.ToString(values.DateOfBirth));
                var result = (db.askmirai_patient_Insert_Update(values.FirstName, values.LastName, values.Email, values.MobileNo == null ? "" : values.MobileNo, values.Gender, values.DateOfBirth, values.CountryId, values.StateId, values.LocationId, values.CityId, values.Password, values.Height, values.Weight, values.Address, values.Pincode, values.UserId, values.RegistrationDate, values.Status, values.UserType, values.UserName, values.IsEmailVerified)).ToList();
                var res = result.FirstOrDefault();
                if (Convert.ToBoolean(res.EmailAvailable))
                {
                    string patientid = Convert.ToString(res.UserId);
                    string emailVerficationURL = ConfigurationManager.AppSettings["EmailVerificationLink"].ToString();
                    string emailBody = EmailTemplates.SendNotificationEmailtoUser(values.FirstName, patientid, emailVerficationURL, "Patient");

                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                    Mail.SendHTMLMailWithImage(fromEmail, values.Email, "Mirai Consult - Verify your email", emailBody, Logoimage);
                    ViewBag.message="Account has been created successfully and you will receive verification email shortly. Please check spam/junk incase you don't find an email in your inbox.";   
                }
                else if (!Convert.ToBoolean(res.EmailAvailable))
                {
                    ViewBag.message = "This email is not available. Please select a different email.";                
                }
            }
            var countryList = poupulateCountry();
            values.Countries = new SelectList(countryList, "countryid", "name");
            values.CountryId = values.CountryId;

            var stateList = poupulateState(Convert.ToInt32(values.CountryId));
            values.States = new SelectList(stateList, "stateId", "name");
            values.StateId = Convert.ToInt32(values.StateId);

            var cityList = poupulateCity(Convert.ToInt32(values.StateId));
            values.Cities = new SelectList(cityList, "cityId", "name");
            values.CityId = Convert.ToInt32(values.CityId);

            var locationList = poupulateLocation(Convert.ToInt32(values.CityId));
            values.Locations = new SelectList(locationList, "locationId", "name");
            values.LocationId = Convert.ToInt32(values.LocationId);

            return View(values);
        }
        [HttpGet]
        public ActionResult DoctorSignUp()
        {

            ModelUser modelUser = new ModelUser();
            var countryList = poupulateCountry();
            modelUser.Countries = new SelectList(countryList, "countryid", "name");
            DataTable dtSpecialities = DAL.UtilityManager.getInstance().getAllSpecialities();
            List<speciality> specialities = new List<speciality>();
            specialities = dtSpecialities.AsEnumerable().Select(dataRow => new speciality
            {
                specialityid = dataRow.Field<int>("specialityid"),
                name = dataRow.Field<string>("name"),
            }).ToList();
            MultiSelectList makeSelected = new MultiSelectList(specialities, "specialityid", "name", specialities);
            ViewBag.specialities = makeSelected;

            return View(modelUser);

        }
        [HttpPost]
        public ActionResult DoctorSignUp(ModelUser modelUser, HttpPostedFileBase file, FormCollection collection)
        {
            DataTable dtDoctor = null;           
            if (ModelState.IsValid)
            {
                User doctor = new User();
                string filename = "";
                if (file != null && !string.IsNullOrEmpty(file.FileName))
                    filename = file.FileName;
                string lstSpeciality = "";
                if (collection != null && collection["specialities"] != null)
                {
                    lstSpeciality = collection["specialities"];
                    string[] specilaity = lstSpeciality.Split(',');
                    foreach (var specialityId in specilaity)
                    {
                        DoctorSpeciality speciality = new DoctorSpeciality();
                        speciality.SpecialityId = Convert.ToInt32(specialityId);
                        doctor.specialities.Add(speciality);
                        //doctor.AddSpeciality(speciality);
                    }
                }
                doctor.Image = filename;
                modelUser.Image=filename;
                doctor.FirstName = modelUser.FirstName;
                doctor.LastName = modelUser.LastName;
                doctor.Gender = Convert.ToInt32(modelUser.Gender);
                if (!string.IsNullOrEmpty(Convert.ToString(modelUser.DateOfBirth)))
                    doctor.DateOfBirth = DateTime.Parse(Convert.ToString(modelUser.DateOfBirth));
                doctor.Email = modelUser.Email;
                doctor.MobileNo = modelUser.MobileNo == null ? "" : modelUser.MobileNo;
                string encpassword = Utilities.Encrypt(modelUser.Password);
                doctor.Password = encpassword;
                doctor.CountryId = Convert.ToInt32(modelUser.CountryId);
                TempData["CountryId"] = doctor.CountryId;
                doctor.RegistrationNumber = modelUser.RegistrationNumber;
                doctor.RegistrationCouncil = Convert.ToInt32(modelUser.Regcouncilid);//modelUser.Regcouncilid);
                doctor.AboutMe = modelUser.AboutMe;
                doctor.Status = Convert.ToInt32(UserStatus.Pending);
                doctor.UserType = Convert.ToInt32(UserType.Doctor);             
                if (filename != "")
                    doctor.PhotoUrl = ConfigurationManager.AppSettings["DoctorPhotosUrl"].ToString().Trim();
                else
                    doctor.PhotoUrl = "";
                dtDoctor = DoctorManager.getInstance().registerdoctor(doctor);
                if (dtDoctor != null && dtDoctor.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtDoctor.Rows[0]["EmailAvailable"]))
                    {

                        if (filename != "")
                        {
                            string strPhysicalFilePath = "";
                            string[] array = { ".PNG", ".JPG", ".GIF", ".JPEG" };
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (filename.EndsWith(array[i]))
                                {
                                    filename = filename.Replace(array[i], array[i].ToLower());
                                    break;
                                }
                            }
                            string ImageUpoading_path = ConfigurationManager.AppSettings["DoctorPhotosPath"].ToString().Trim();
                            string onlyFile = filename.Substring(filename.LastIndexOf('\\') + 1);
                            if (ImageUpoading_path != "")
                            {
                                strPhysicalFilePath = ImageUpoading_path + @"\" + Convert.ToString(dtDoctor.Rows[0]["UserId"]) + onlyFile;
                                if (!Directory.Exists(ImageUpoading_path.Trim()))
                                {
                                    Directory.CreateDirectory(ImageUpoading_path.Trim());
                                }
                                if (!System.IO.File.Exists(strPhysicalFilePath))
                                {
                                    var path = Path.Combine(ImageUpoading_path, filename);
                                    file.SaveAs(path);
                                }
                                else
                                {
                                    System.IO.File.Delete(strPhysicalFilePath);
                                    var path = Path.Combine(ImageUpoading_path, filename);
                                    file.SaveAs(path);
                                }
                            }
                        }
                        string doctorid = Convert.ToString(dtDoctor.Rows[0]["UserId"]);
                        string emailVerficationURL = ConfigurationManager.AppSettings["EmailVerificationLink"].ToString();
                        string emailBody = EmailTemplates.SendNotificationEmailtoUser(doctor.FirstName, doctorid, emailVerficationURL, "Doctor");
                        string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                        string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                        Mail.SendHTMLMailWithImage(fromEmail, modelUser.Email, "Mirai Consult - Verify your email", emailBody, Logoimage);
                        ViewBag.message = "Your registration request has been submitted successfully. You will receive verification email shortly. Please check spam/junk incase you don't find an email in your inbox.";
                    }
                    else
                    {
                        ViewBag.message = "This email is not available. Please select a different email.";
                    }
                }
                else
                {
                    //errorMessageDiv.Visible = false;
                    //return;
                }
            }
            modelUser.hdnRegcouncilid=modelUser.Regcouncilid;
            var countryList = poupulateCountry();
            modelUser.Countries = new SelectList(countryList, "countryid", "name");
            var regconlist = new DoctorsController().PopulateRegCouncilByCountry(Convert.ToInt32(TempData["CountryId"]));
            modelUser.Councils = new SelectList(regconlist, "regcouncilid", "name");
            DataTable dtSpecialities = DAL.UtilityManager.getInstance().getAllSpecialities();
            List<speciality> specialities = new List<speciality>();
            specialities = dtSpecialities.AsEnumerable().Select(dataRow => new speciality
            {
                specialityid = dataRow.Field<int>("specialityid"),
                name = dataRow.Field<string>("name"),
            }).ToList();
            MultiSelectList makeSelected = new MultiSelectList(specialities, "specialityid", "name", specialities);
            ViewBag.specialities = makeSelected;
            return View(modelUser);
        }
        [HttpGet]
        public IList<Country> poupulateCountry()
        {
            IList<Country> countryLst = new List<Country>();
            var countrylist = db.countries.ToList().OrderBy(c => c.name);
            if (countrylist != null && countrylist.Count() > 0)
            {
                foreach (var country in countrylist)
                {
                    Country country1 = new Country();
                    country1.countryid = Convert.ToInt32(country.countryid);
                    country1.name = Convert.ToString(country.name);
                    country1.countrycode = Convert.ToString(country.countrycode);
                    countryLst.Add(country1);
                }
            }
            return countryLst;
        }
        public IList<State> poupulateState(int countryId)
        {
            IList<State> stateLst = new List<State>();
            var stateList = db.states.Where(s => s.countryid.Equals(countryId)).ToList().OrderBy(c => c.name);
            if (stateList != null && stateList.Count() > 0)
            {
                foreach (var state in stateList)
                {
                    State state1 = new State();
                    state1.stateid = Convert.ToInt32(state.stateid);
                    state1.countryid = Convert.ToInt32(state.countryid);
                    state1.name = Convert.ToString(state.name);
                    stateLst.Add(state1);
                }
            }
            return stateLst;
        }

        public IList<City> poupulateCity(int stateId)
        {
            IList<City> cityLst = new List<City>();
            var cityList = db.cities.Where(c => c.stateid.Equals(stateId)).ToList().OrderBy(c => c.name);
            if (cityList != null && cityList.Count() > 0)
            {
                foreach (var city in cityList)
                {
                    City city1 = new City();
                    city1.cityid = Convert.ToInt32(city.cityid);
                    city1.stateid = Convert.ToInt32(city.stateid);
                    city1.name = Convert.ToString(city.name);
                    cityLst.Add(city1);
                }
            }
            return cityLst;
        }

        public IList<Location> poupulateLocation(int cityId)
        {
            IList<Location> locationLst = new List<Location>();
            var locationList = db.locations.Where(c => c.cityid.Equals(cityId)).ToList().OrderBy(c => c.name);
            if (locationList != null && locationList.Count() > 0)
            {
                foreach (var location in locationList)
                {
                    Location location1 = new Location();
                    location1.locationid = Convert.ToInt32(location.locationid);
                    location1.cityid = Convert.ToInt32(location.cityid);
                    location1.name = Convert.ToString(location.name);
                    locationLst.Add(location1);
                }
            }
            return locationLst;
        }

        [HttpGet]
        public JsonResult poupulateStateByCountry(int countryId)
        {
            IList<State> stateLst = new List<State>();
            var stateList = db.states.Where(s => s.countryid.Equals(countryId)).ToList().OrderBy(c => c.name);
            if (stateList != null && stateList.Count() > 0)
            {
                foreach (var state in stateList)
                {
                    State state1 = new State();
                    state1.stateid = Convert.ToInt32(state.stateid);
                    state1.countryid = Convert.ToInt32(state.countryid);
                    state1.name = Convert.ToString(state.name);
                    stateLst.Add(state1);
                }
            }
            return Json(stateLst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult poupulateCityByState(int stateId)
        {
            IList<City> cityLst = new List<City>();
            var cityList = db.cities.Where(s => s.stateid.Equals(stateId)).ToList().OrderBy(c => c.name);
            if (cityList != null && cityList.Count() > 0)
            {
                foreach (var city in cityList)
                {
                    City city1 = new City();
                    city1.cityid = Convert.ToInt32(city.cityid);
                    city1.stateid = Convert.ToInt32(city.stateid);
                    city1.name = Convert.ToString(city.name);
                    cityLst.Add(city1);
                }
            }
            return Json(cityLst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult poupulateLocationByCity(int cityId)
        {
            IList<Location> locationLst = new List<Location>();
            var locationList = db.locations.Where(c => c.cityid.Equals(cityId)).ToList().OrderBy(c => c.name);
            if (locationList != null && locationList.Count() > 0)
            {
                foreach (var location in locationList)
                {
                    Location location1 = new Location();
                    location1.locationid = Convert.ToInt32(location.locationid);
                    location1.cityid = Convert.ToInt32(location.cityid);
                    location1.name = Convert.ToString(location.name);
                    locationLst.Add(location1);
                }
            }
            return Json(locationLst, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult populateRegistrationCouncilbyCountry(int countryId)
        {
            IList<RegistrationCouncil> councilLst = new List<RegistrationCouncil>();
            var councilList = db.registrationcouncils.Where(s => s.countryid.Equals(countryId)).ToList().OrderBy(c => c.name);
            if (councilList != null && councilList.Count() > 0)
            {
                foreach (var council in councilList)
                {
                    RegistrationCouncil RegistrationCouncil1 = new RegistrationCouncil();
                    RegistrationCouncil1.regcouncilid = Convert.ToInt32(council.regcouncilid);
                    RegistrationCouncil1.countryid = Convert.ToInt32(council.countryid);
                    RegistrationCouncil1.name = Convert.ToString(council.name);
                    councilLst.Add(RegistrationCouncil1);
                }
            }
            return Json(councilLst, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public IList<Specialities> poupulateSpeciality()
        {
            IList<Specialities> specialityLst = new List<Specialities>();
            var specialitylist = db.specialities.ToList().OrderBy(c => c.name);
            if (specialitylist != null && specialitylist.Count() > 0)
            {
                foreach (var speciality in specialitylist)
                {
                    Specialities speciality1 = new Specialities();
                    speciality1.specialityid = Convert.ToInt32(speciality.specialityid);
                    speciality1.name = Convert.ToString(speciality.name);
                    specialityLst.Add(speciality1);
                }
            }
            return specialityLst;
        }

        public string AutoComplete(string term)
        {
            DataSet dsQuestions = QuestionManager.getInstance().searchQuestion(term, Convert.ToInt32(QuestionStatus.Approved));
            return JsonConvert.SerializeObject(dsQuestions.Tables[0]);
        }

        public int showUnansweredQuestionCount()
        {
            if (Session["UserId"] != null && Session["UserType"] != null && Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Doctor))
            {
                return QuestionManager.getInstance().getUnansweredQuestionCount(Convert.ToInt32(Session["UserId"]));
            }
            return 0;
        }

        private void setUserPrivilegesBasedOnUsertype(int userType)
        {
            List<int> privileges = new List<int>();
            switch (userType)
            {
                case 2: privileges.Add(Convert.ToInt32(Privileges.askdoctor)); // for patient
                    privileges.Add(Convert.ToInt32(Privileges.patientprofile));
                    privileges.Add(Convert.ToInt32(Privileges.patientfeed));
                    privileges.Add(Convert.ToInt32(Privileges.changepassword));
                    privileges.Add(Convert.ToInt32(Privileges.Invitefriend));
                    privileges.Add(Convert.ToInt32(Privileges.Myactivity));
                    break;
                case 1: privileges.Add(Convert.ToInt32(Privileges.changepassword)); // for doctor
                    privileges.Add(Convert.ToInt32(Privileges.doctorquestiondetails));
                    privileges.Add(Convert.ToInt32(Privileges.questionlist));
                    privileges.Add(Convert.ToInt32(Privileges.Invitefriend));
                    privileges.Add(Convert.ToInt32(Privileges.doctorprofile));
                    break;
                case 0: privileges.Add(Convert.ToInt32(Privileges.patientfeed)); // for superadmin
                    privileges.Add(Convert.ToInt32(Privileges.doctorprofile));
                    privileges.Add(Convert.ToInt32(Privileges.manageDoctor));
                    privileges.Add(Convert.ToInt32(Privileges.questionlist));
                    privileges.Add(Convert.ToInt32(Privileges.Invitefriend));
                    privileges.Add(Convert.ToInt32(Privileges.managetags));
                    privileges.Add(Convert.ToInt32(Privileges.assignQuestion));
                    privileges.Add(Convert.ToInt32(Privileges.reports));
                    break;
            }
            Session["privileges"] = privileges;
        }

    }
}
