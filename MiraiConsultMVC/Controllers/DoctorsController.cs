using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;

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
        public ActionResult DoctorProfile()
        {
            BPage.isAuthorisedandSessionExpired(Convert.ToInt32(Privileges.doctorprofile));
            return View(getDoctorDetailsByDoctorId(6));
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
        public DoctorProfile getDoctorDetailsByDoctorId(int userid)
        {
            DoctorProfile doctorDetail = new DoctorProfile();
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();

            var doctor = db.users.FirstOrDefault(p => p.userid.Equals(userid));
            if (doctor != null)
            {
                doctorDetail.UserId = Convert.ToInt32(doctor.userid);
                doctorDetail.FirstName = Convert.ToString(doctor.firstname);
                doctorDetail.LastName = Convert.ToString(doctor.lastname);
                doctorDetail.Email = Convert.ToString(doctor.email);
                if (doctor.mobileno != null)
                    doctorDetail.MobileNo = Convert.ToString(doctor.mobileno);
                if (doctor.gender != null)
                    doctorDetail.Gender = Convert.ToInt32(doctor.gender);
                if (doctor.address != null)
                    doctorDetail.Address = Convert.ToString(doctor.address);
                  
                doctorDetail.DateOfBirth = Convert.ToDateTime(doctor.dateofbirth);
                if (doctor.countryid != null && doctor.countryid != 0)
                {
                    var countryList = poupulateCountry();
                    ViewBag.countries = new SelectList(countryList, "countryid", "name");
                    doctorDetail.CountryId = Convert.ToInt32(doctor.countryid);
                }

                if (doctor.username != null)
                    doctorDetail.UserName = Convert.ToString(doctor.username);
                if (doctor.registrationdate != null)
                    doctorDetail.RegistrationDate = Convert.ToDateTime(doctor.registrationdate);
                if (doctor.status != null)
                    doctorDetail.Status = Convert.ToInt32(doctor.status);
                if (doctor.isemailverified != null)
                    doctorDetail.IsEmailVerified = Convert.ToBoolean(doctor.isemailverified);
            }
            return doctorDetail;
        }



        


    }
}
