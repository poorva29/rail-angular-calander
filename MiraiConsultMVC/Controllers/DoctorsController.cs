using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using DAL;
using Model;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace MiraiConsultMVC.Controllers
{
    public class DoctorsController : Controller
    {
        //
        // GET: /Doctors/
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        BasePage BPage = new BasePage();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoctorProfile(string UserId=null)
        {
            int userId;
            int privilege = BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.doctorprofile));
            if (privilege == 1)
            {
                return RedirectToAction("NoPrivilegeError", "Home");
            }
            else
            {
                if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.SuperAdmin) && UserId == null)
                {
                    userId = Convert.ToInt32(Session["DoctorId"]);
                }
                else if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.SuperAdmin))
                {
                    userId = Convert.ToInt32(Utilities.Decrypt(HttpUtility.UrlDecode(UserId.ToString()).Replace(" ", "+")));
                    Session["DoctorId"] = userId;
                }
                else
                {
                    userId = Convert.ToInt32(Session["UserId"]);
                }
                return View(getDoctorDetailsByDoctorId(userId));
            }
        }
        [HttpGet]
        public IList<Country> poupulateCountry()
        {
            IList<Country> countryLst = new List<Country>();
            DataTable countrylist = DAL.UtilityManager.getInstance().getAllCountries();
            if (countrylist != null && countrylist.Rows.Count > 0)
            {
                foreach (DataRow country in countrylist.Rows)
                {
                    Country country1 = new Country();
                    country1.countryid = Convert.ToInt32(country["countryid"]);
                    country1.name = Convert.ToString(country["name"]);
                    countryLst.Add(country1);
                }
            }
            return countryLst;
        }
        [HttpGet]
        public IList<speciality> poupulateSpeciality()
        {
            IList<speciality> specialityLst = new List<speciality>();
            var specialitylist = db.specialities.ToList().OrderBy(c => c.name);
            if (specialitylist != null && specialitylist.Count() > 0)
            {
                foreach (var speciality in specialitylist)
                {
                    speciality obj_speciality = new speciality();
                    obj_speciality.specialityid = Convert.ToInt32(speciality.specialityid);
                    obj_speciality.name = Convert.ToString(speciality.name);
                    specialityLst.Add(obj_speciality);
                }
            }
            return specialityLst;
        }

        [HttpGet]
        public IList<RegistrationCouncil> PopulateRegCouncilByCountry(int countryId)
        {
            IList<RegistrationCouncil> regCouncilLst = new List<RegistrationCouncil>();
            var registrationCouncillist = db.registrationcouncils.ToList().OrderBy(c => c.name);
            if (registrationCouncillist != null && registrationCouncillist.Count() > 0)
            {
                foreach (var registrationCouncil in registrationCouncillist)
                {
                    RegistrationCouncil reg_council = new RegistrationCouncil();
                    reg_council.regcouncilid = Convert.ToInt32(registrationCouncil.regcouncilid);
                    reg_council.name = Convert.ToString(registrationCouncil.name);
                    regCouncilLst.Add(reg_council);
                }
            }
            return regCouncilLst;
        }

        [HttpGet]
        public DoctorProfile getDoctorDetailsByDoctorId(int userId)
        {
            DoctorProfile doctorDetail = new DoctorProfile();
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            var doctor = DoctorManager.getInstance().getDoctorDetailsById(userId);
            //var doctor = db.users.FirstOrDefault(p => p.userid.Equals(userid));
            if (doctor != null)
            {
                doctorDetail.UserId = Convert.ToInt32(doctor.UserId);
                doctorDetail.FirstName = Convert.ToString(doctor.FirstName);
                doctorDetail.LastName = Convert.ToString(doctor.LastName);
                doctorDetail.Email = Convert.ToString(doctor.Email);
                TempData["Email"] = Convert.ToString(doctor.Email);
                if (doctor.MobileNo != null)
                    doctorDetail.MobileNo = Convert.ToString(doctor.MobileNo);
                if (doctor.Gender != null)
                    doctorDetail.Gender = Convert.ToInt32(doctor.Gender);
                if (doctor.Address != null)
                    doctorDetail.Address = Convert.ToString(doctor.Address);
                if (doctor.DateOfBirth != null)
                    doctorDetail.DateOfBirth = Convert.ToDateTime(doctor.DateOfBirth);
                if (doctor.CountryId >= 0)
                {
                    var countryList = poupulateCountry();
                    doctorDetail.Countries = new SelectList(countryList, "countryid", "name");
                    doctorDetail.CountryId = Convert.ToInt32(doctor.CountryId);
                }
                if (doctor.RegistrationCouncil > 0)
                {
                    TempData["countryId"] = doctor.CountryId;
                    var regCouncilList = PopulateRegCouncilByCountry(Convert.ToInt32(doctor.CountryId));
                    ViewBag.Registrationcouncils = new SelectList(regCouncilList, "regcouncilid", "name");
                    doctorDetail.RegistrationCouncil = Convert.ToInt32(doctor.RegistrationCouncil);
                }
                DataTable dtSpecialities = UtilityManager.getInstance().getAllSpecialities();

                List<DoctorSpeciality> specialities = new List<DoctorSpeciality>();

                specialities = dtSpecialities.AsEnumerable().Select(dataRow => new DoctorSpeciality
                {
                    SpecialityId = dataRow.Field<int>("specialityid"),
                    Speciality = dataRow.Field<string>("name"),
                }).ToList();

                IList<DoctorSpeciality> SelectedSpecialities = new List<DoctorSpeciality>();
                SelectedSpecialities = doctor.specialities;

                int[] values = new int[SelectedSpecialities.Count];
                for (int i = 0; i < SelectedSpecialities.Count; i++)
                {
                    values[i] = SelectedSpecialities.ToList()[i].SpecialityId;
                }

                MultiSelectList makeSelected = new MultiSelectList(specialities, "SpecialityId", "Speciality", values);
                ViewBag.specialities = makeSelected;

                if (doctor.RegistrationNumber != null)
                    doctorDetail.RegistrationNumber = doctor.RegistrationNumber;
                if (doctor.AboutMe != null)
                    doctorDetail.AboutMe = doctor.AboutMe;
                if (doctor.UserName != null)
                    doctorDetail.UserName = Convert.ToString(doctor.UserName);
                if (doctor.RegistrationDate != null)
                    doctorDetail.RegistrationDate = Convert.ToDateTime(doctor.RegistrationDate);
                if (doctor.Status != null)
                    doctorDetail.Status = Convert.ToInt32(doctor.Status);
                if (doctor.IsEmailVerified != null)
                    doctorDetail.IsEmailVerified = Convert.ToBoolean(doctor.IsEmailVerified);
                if (doctor.Image != null)
                {
                    doctorDetail.Image = doctor.PhotoUrl + doctor.Image;
                    TempData["Image"] = doctor.Image;
                }
                else
                    doctorDetail.Image = "\\Content\\image\\img-na.png";
                if (doctor.qualification != null)
                {
                    foreach (var qualification in doctor.qualification)
                    {
                        doctorqualifications doctorqualification = new doctorqualifications();
                        if (!String.IsNullOrEmpty(Convert.ToString(qualification.DegreeId)))
                        {
                            doctorqualification.DegreeId = Convert.ToInt32(qualification.DegreeId);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(qualification.Degree)))
                        {
                            doctorqualification.Degree = Convert.ToString(qualification.Degree);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(qualification.University)))
                        {
                            doctorqualification.University = Convert.ToString(qualification.University);

                        }
                        doctorDetail.qualification.Add(doctorqualification);
                    }
                }
                if (doctor.locations != null)
                {
                    foreach (var locations in doctor.locations)
                    {
                        DoctorLocations doctorlocation = new DoctorLocations();
                        doctorlocation.DoctorLocationId = locations.DoctorLocationId;
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.CountryId)))
                        {
                            doctorlocation.CountryId = Convert.ToInt32(locations.CountryId);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.Country)))
                        {
                            doctorlocation.Country = Convert.ToString(locations.Country);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.StateId)))
                        {
                            doctorlocation.StateId = Convert.ToInt32(locations.StateId);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.State)))
                        {
                            doctorlocation.State = Convert.ToString(locations.State);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.CityId)))
                        {
                            doctorlocation.CityId = Convert.ToInt32(locations.CityId);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.City)))
                        {
                            doctorlocation.City = Convert.ToString(locations.City);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.LocationId)))
                        {
                            doctorlocation.LocationId = Convert.ToInt32(locations.LocationId);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.ClinicName)))
                        {
                            doctorlocation.ClinicName = Convert.ToString(locations.ClinicName);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.Location)))
                        {
                            doctorlocation.Location = Convert.ToString(locations.Location);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(locations.Telephone)))
                        {
                            doctorlocation.Telephone = Convert.ToString(locations.Telephone);
                        }
                        if (!String.IsNullOrEmpty(locations.Address))
                        {
                            doctorlocation.Address = locations.Address;
                        }
                        doctorDetail.locations.Add(doctorlocation);
                    }
                }
                if (doctor.details != null)
                {
                    foreach (var detail in doctor.details)
                    {
                        doctordetails doctordetails = new doctordetails();
                        if (!String.IsNullOrEmpty(Convert.ToString(detail.DocDetailsId)))
                        {
                            doctordetails.DocDetailsId = Convert.ToInt32(detail.DocDetailsId);
                        }

                        if (!String.IsNullOrEmpty(Convert.ToString(detail.UserId)))
                        {
                            doctordetails.UserId = Convert.ToInt32(detail.UserId);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(detail.Certification)))
                        {
                            doctordetails.Certification = Convert.ToString(detail.Certification);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(detail.Society)))
                        {
                            doctordetails.Society = Convert.ToString(detail.Society);
                        }
                        doctorDetail.details.Add(doctordetails);
                    }
                }
            }
            return doctorDetail;
        }

        public JsonResult UpdateDegreeByDoctor(string doctorId, string LastSelectedDegreeID, string SelectedDegreeId, string university)
        {
            int result = 0;
            result = DoctorManager.getInstance().UpdateDegreeUniversityByDoctorAndDegrreId(doctorId, LastSelectedDegreeID, SelectedDegreeId, university);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDegreeByDoctor(string doctorId, string LastSelectedDegreeID, string university)
        {
            int result = 0;
            result = DoctorManager.getInstance().DeleteDegreeUniversityByDoctorAndDegrreId(doctorId, LastSelectedDegreeID, university);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDoctorDetailsByDoctorDetailsId(string doctorId, string doctordetailsid, string SelectedTypeId, string Details)
        {
            int result = 0;
            result = DoctorManager.getInstance().UpdateDoctorDetailsByDoctorDetailsId(doctorId, doctordetailsid, SelectedTypeId, Details);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDoctorDetailsByDoctorDetailsId(string doctordetailsid)
        {
            int result = 0;
            result = DoctorManager.getInstance().DeleteDoctorDetailsByDoctorDetailsId(doctordetailsid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Location_Insert(int doctorid, string address, string telephone, string clinicName, string countryId, string stateId, string cityId, string locationId, string docLocationId)
        {
            int docLocID;
            if (!String.IsNullOrEmpty(docLocationId))
            {
                docLocID = Convert.ToInt32(docLocationId);
            }
            else
            {
                docLocID = 0;
            }
            DoctorLocations docLocation = new DoctorLocations();
            docLocation.DoctorLocationId = docLocID;
            if (!string.IsNullOrEmpty(address))
            {
                docLocation.Address = address;
            }
            if (!string.IsNullOrEmpty(telephone))
            {
                docLocation.Telephone = telephone;
            }
            if (!string.IsNullOrEmpty(clinicName))
            {
                docLocation.ClinicName = clinicName;
            }
            if (!string.IsNullOrEmpty(countryId))
            {
                docLocation.CountryId = Convert.ToInt32(countryId);
            }
            if (!string.IsNullOrEmpty(stateId))
            {
                docLocation.StateId = Convert.ToInt32(stateId);
            }
            if (!string.IsNullOrEmpty(cityId))
            {
                docLocation.CityId = Convert.ToInt32(cityId);
            }
            if (!string.IsNullOrEmpty(locationId))
            {
                docLocation.LocationId = Convert.ToInt32(locationId);
            }
            DataSet ds = DoctorManager.getInstance().AddUpdateDoctorClinic(doctorid, docLocation);
            if (ds != null && ds.Tables.Count > 0)
            {
                docLocID = Convert.ToInt32(ds.Tables[0].Rows[0]["doclocid"]);
                if (docLocID != 0)
                {
                    TempData["clinicMessage"] = docLocationId.Equals("0") ? "New clinic details added successfully." : "Clinic details updated successfully";
                }
            }
            return Json(docLocationId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDoctorLocationByDoctorLocationId(string docLocationId)
        {
            int result = 0;
            result = DoctorManager.getInstance().DeleteDoctorLocattonByDoctorLocationId(docLocationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DoctorProfile(DoctorProfile profile, FormCollection collection, HttpPostedFileBase file)
        {
            DataTable dtDoctor = new DataTable();
            User doctor = new User();
            if (ModelState.IsValid)
            {
                if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.SuperAdmin))
                {
                    doctor.UserId = Convert.ToInt32(Session["DoctorId"]);
                }
                else
                doctor.UserId = Convert.ToInt32(Session["UserId"]);
                doctor.Email = profile.Email;
                if (!TempData["Email"].Equals(profile.Email))
                    doctor.IsEmailVerified = false;
                else
                    doctor.IsEmailVerified = true;
                doctor.Gender = profile.Gender;
                doctor.FirstName = profile.FirstName;
                doctor.LastName = profile.LastName;
                doctor.MobileNo = profile.MobileNo == null ? "" : profile.MobileNo;
                if (profile.DateOfBirth != null)
                    doctor.DateOfBirth = Convert.ToDateTime(profile.DateOfBirth);
                doctor.RegistrationDate = System.DateTime.Now;
                doctor.UserName = profile.UserName;
                doctor.RegistrationNumber = profile.RegistrationNumber;
                string docOldImage = Convert.ToString(TempData["Image"]);
                string filename = "";
                filename = file != null ? file.FileName : "";
                filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                if (file != null && !string.IsNullOrEmpty(filename))
                {
                    if (Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.SuperAdmin))
                    {
                        filename = Convert.ToInt32(Session["DoctorId"]) + filename;
                    }
                    else
                        filename = Convert.ToString(Session["UserId"]) + filename;
                }
                if (file != null && file.FileName != null)
                {
                    doctor.PhotoUrl = ConfigurationManager.AppSettings["DoctorPhotosUrl"].ToString().Trim();
                    doctor.Image = filename;
                }
                else
                {
                    doctor.PhotoUrl = ConfigurationManager.AppSettings["DoctorPhotosUrl"].ToString().Trim();
                    doctor.Image = docOldImage;
                }
                
               
                if (Convert.ToInt32(profile.CountryId) != 0)
                {
                    doctor.CountryId = Convert.ToInt32(profile.CountryId);
                }
                if (Convert.ToInt32(profile.RegistrationCouncil) != 0)
                {
                    doctor.RegistrationCouncil = Convert.ToInt32(profile.RegistrationCouncil);
                }
                doctor.AboutMe = profile.AboutMe;

                var lstSpeciality = collection["lstspecialities"];
                List<string> specilaity = new List<string>();
                if (lstSpeciality != null)
                    specilaity = lstSpeciality.Split(',').ToList();
                foreach (var specialityId in specilaity)
                {
                    DoctorSpeciality speciality = new DoctorSpeciality();
                    speciality.SpecialityId = Convert.ToInt32(specialityId);
                    doctor.AddSpeciality(speciality);
                }
                dtDoctor = DoctorManager.getInstance().UpdatedoctordetailById(doctor);
                if (dtDoctor != null && dtDoctor.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtDoctor.Rows[0]["EmailAvailable"]))
                    {
                        string usertype = Convert.ToString(Session["UserType"]);
                        if (!TempData["Email"].Equals(doctor.Email))
                        {
                            bool isemailverfiy = true;
                            string doctorid = Convert.ToString(dtDoctor.Rows[0]["UserId"]);
                            String Usertype = "Doctor";
                            string emailVerficationURL = ConfigurationManager.AppSettings["EmailVerificationLink"].ToString();
                            string emailid = doctor.Email;
                            string emailBody = EmailTemplates.SendEmailVerifcationtoUser(profile.LastName, doctorid, emailVerficationURL, Usertype, emailid, isemailverfiy);
                            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                            string Logoimage = Server.MapPath(@"~/Content/image/LogoForMail.png");
                            Mail.SendHTMLMailWithImage(fromEmail, profile.Email, "Mirai Consult - Verify your email", emailBody, Logoimage);
                            TempData["message"] = "Details updated successfully. You will receive verification email shortly.";
                            TempData["Email"] = doctor.Email;
                            TempData["Image"] = doctor.Image;
                        }
                        else
                        {
                            TempData["message"] = "Details updated successfully.";
                            TempData["Image"] = doctor.Image;
                        }
                        if (filename != "")
                        {
                            string strPhysicalFilePath = "";
                            string strPhysicalFilePatholdimg = "";
                            string oldimag = docOldImage;
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
                            string oldfile = oldimag.Substring(oldimag.LastIndexOf('\\') + 1);

                            if (ImageUpoading_path != "")
                            {
                                strPhysicalFilePath = ImageUpoading_path + @"\" + onlyFile;
                                strPhysicalFilePatholdimg = ImageUpoading_path + @"\" + oldfile;
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
                        if (Convert.ToInt32(usertype) == Convert.ToInt32(UserType.Doctor))
                        {
                            HttpContext.Session["UserFirstName"] = doctor.FirstName;
                            HttpContext.Session["UserLastName"] = doctor.LastName;
                            HttpContext.Session["UserFullName"] = doctor.FirstName + " " + doctor.LastName;
                            HttpContext.Session["UserMobileNo"] = doctor.MobileNo;
                            HttpContext.Session["IsEmailVerified"] = doctor.IsEmailVerified;
                        }
                    }
                    else
                    {
                        TempData["message"] = "Other User with same email address already exist. Please enter other email.";
                    }
                }
            }
            return RedirectToAction("DoctorProfile");
        }
    }
}
