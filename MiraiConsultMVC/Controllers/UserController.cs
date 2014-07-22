using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models.User;
using MiraiConsultMVC.Models;
using System.Configuration;
using MiraiConsultMVC.Models.Utilities;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Reflection;
using System.Data.Linq.Mapping;


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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login log)
        {
            //Utilities U = new Utilities();
            string SuperAdminEmailId = ConfigurationManager.AppSettings["SuperAdminEmailId"]; // Please make sure that this username doesn't exist in Patient, Doctor, DoctorAssistant table
            string SuperAdminUserPassword = ConfigurationManager.AppSettings["SuperAdminUserPassword"].ToString();
            string dbpasswd = MiraiConsultMVC.Models.Utilities.UtilityManager.Decrypt(log.Password);
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
            IMultipleResults lstdoctors = db.getAllDoctorDetails();

            return View();
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
        //public ActionResult GetCounsilList(int CountryId)
        //{ 
        //return 
        //}

    }
}
