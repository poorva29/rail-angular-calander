using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using DAL;
using Model;
using System.Data;

namespace MiraiConsultMVC.Controllers
{
    public class DoctorsController : Controller
    {
        //
        // GET: /Doctors/
        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoctorProfile()
        {
            return View(getDoctorDetailsByDoctorId(Convert.ToInt32(Session["UserId"])));
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
                if (doctor.MobileNo != null)
                    doctorDetail.MobileNo = Convert.ToString(doctor.MobileNo);
                if (doctor.Gender != null)
                    doctorDetail.Gender = Convert.ToInt32(doctor.Gender);
                if (doctor.Address != null)
                    doctorDetail.Address = Convert.ToString(doctor.Address);
                if (doctor.DateOfBirth !=null) 
                    doctorDetail.DateOfBirth = Convert.ToDateTime(doctor.DateOfBirth);
                if (doctor.CountryId != null && doctor.CountryId != 0)
                {
                    var countryList = poupulateCountry();
                    ViewBag.countries = new SelectList(countryList, "countryid", "name");
                    doctorDetail.CountryId = Convert.ToInt32(doctor.CountryId);
                }
                if(doctor.RegistrationCouncil != null)
                {
                    var regCouncilList = PopulateRegCouncilByCountry(doctor.CountryId);
                    ViewBag.Registrationcouncils = new SelectList(regCouncilList, "regcouncilid", "name");
                    doctorDetail.RegistrationCouncil = Convert.ToInt32(doctor.RegistrationCouncil);
                }
                if(doctor.specialities != null)
                {
                    var specialityList = poupulateSpeciality();
                    ViewBag.speciality = new SelectList(specialityList, "specialityid", "name");
                }
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
                if (doctor.PhotoUrl != null)
                    doctorDetail.PhotoUrl = doctor.PhotoUrl;              
                if (doctor.qualification != null)
                {
                    foreach(var qualification in doctor.qualification)
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

        public JsonResult Location_Insert(int doctorid, string address, string telephone, string clinicName, string countryId, string stateId, string cityId, string locationId,string docLocationId)
        {
            int docLocID;
            string msg = "";
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
            if(!string.IsNullOrEmpty(address))
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
                    msg = docLocationId.Equals("0") ? "New clinic details added successfully." : "Clinic details updated successfully";
                }
            }
            return Json(docLocationId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDoctorLocattonByDoctorLocationId(string docLocationId)
        {
            int result = 0;
            result = DoctorManager.getInstance().DeleteDoctorLocattonByDoctorLocationId(docLocationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
