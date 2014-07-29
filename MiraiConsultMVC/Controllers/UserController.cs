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
using MiraiConsultMVC.Models.Utilities;
using Model;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MiraiConsultMVC.Controllers
{
    public class UserController : Controller
    {
       
        //
        // GET: /User/
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
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
            //Utilities U = new Utilities()
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
                string dbpasswd = MiraiConsultMVC.Models.Utilities.UtilityManager.Encrypt(passwords.Password);
                var userRecord = db.users.FirstOrDefault(x => x.userid.Equals(userID));
                if (userRecord != null)
                {
                    userRecord.password = MiraiConsultMVC.Models.Utilities.UtilityManager.Encrypt(passwords.Password); ;
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
        public ActionResult PatientSignUp(ModelUser modelUser)
        {
            if (ModelState.IsValid)
            {
                modelUser.Password = Utilities.Encrypt(modelUser.Password);
                modelUser.RegistrationDate = DateTime.Now;
                modelUser.UserType = Convert.ToInt32(UserType.Patient);
                modelUser.UserId = Convert.ToInt32(Session["UserId"]);
                modelUser.Status = Convert.ToInt32(UserStatus.Pending);
                modelUser.IsEmailVerified = false;
                modelUser.DateOfBirth = DateTime.Parse(Convert.ToString(modelUser.DateOfBirth));
                var result = (db.askmirai_patient_Insert_Update(modelUser.FirstName, modelUser.LastName, modelUser.Email, modelUser.MobileNo, modelUser.Gender, modelUser.DateOfBirth, modelUser.CountryId, modelUser.StateId, modelUser.LocationId, modelUser.CityId, modelUser.Password, modelUser.Height, modelUser.Weight, modelUser.Address, modelUser.Pincode, modelUser.UserId, modelUser.RegistrationDate, modelUser.Status, modelUser.UserType, modelUser.UserName, modelUser.IsEmailVerified)).ToList();
                var res = result.FirstOrDefault();
                if (Convert.ToBoolean(res.EmailAvailable))
                {
                    string patientid = Convert.ToString(res.UserId);
                    string emailVerficationURL = ConfigurationManager.AppSettings["EmailVerificationLink"].ToString();
                    string emailBody = EmailTemplates.SendNotificationEmailtoUser(modelUser.FirstName, patientid, emailVerficationURL, "Patient");

                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                    Mail.SendHTMLMailWithImage(fromEmail, modelUser.Email, "Mirai Consult - Verify your email", emailBody, Logoimage);
                    ViewBag.message="Account has been created successfully and you will receive verification email shortly. Please check spam/junk incase you don't find an email in your inbox.";   
                }
                else if (!Convert.ToBoolean(res.EmailAvailable))
                {
                    ViewBag.message = "This email is not available. Please select a different email.";                
                }
            }
            var countryList = poupulateCountry();
            modelUser.Countries = new SelectList(countryList, "countryid", "name");
            modelUser.CountryId = modelUser.CountryId;

            var stateList = poupulateState(Convert.ToInt32(modelUser.CountryId));
            modelUser.States = new SelectList(stateList, "stateId", "name");
            modelUser.StateId = Convert.ToInt32(modelUser.StateId);

            var cityList = poupulateCity(Convert.ToInt32(modelUser.StateId));
            modelUser.Cities = new SelectList(cityList, "cityId", "name");
            modelUser.CityId = Convert.ToInt32(modelUser.CityId);

            var locationList = poupulateLocation(Convert.ToInt32(modelUser.CityId));
            modelUser.Locations = new SelectList(locationList, "locationId", "name");
            modelUser.LocationId = Convert.ToInt32(modelUser.LocationId);

            return View(modelUser);
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
        public ActionResult DoctorSignUp(ModelUser modelUser, HttpPostedFileBase file)
        {
            DataTable dtDoctor = null;
            if (ModelState.IsValid)
            {
                User doctor = new User();
                string filename = "";
                if (filename != "")
                {
                    filename = file.FileName;
                }
                //foreach (ListItem li in lstSpecialities.Items)
                //{
                //    if (li.Selected)
                //    {
                //        DoctorSpecialities speciality = new DoctorSpecialities();
                //        speciality.SpecialityId = Convert.ToInt32(li.Value);
                //        doctor.AddSpeciality(speciality);
                //    }
                //}
                doctor.Image = filename;
                modelUser.Image=filename;
                doctor.FirstName = modelUser.FirstName;
                doctor.LastName = modelUser.LastName;
                doctor.Gender = modelUser.Gender;
                doctor.DateOfBirth = DateTime.Parse(Convert.ToString(modelUser.DateOfBirth));
                doctor.Email = modelUser.Email;
                doctor.MobileNo = modelUser.MobileNo;
                string encpassword = Utilities.Encrypt(modelUser.Password);
                doctor.Password = encpassword;
                doctor.CountryId = Convert.ToInt32(modelUser.CountryId);
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
            var specialityList = poupulateSpeciality();
            modelUser.Specialities = new SelectList(specialityList, "specialityid", "name");
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
    }
}
