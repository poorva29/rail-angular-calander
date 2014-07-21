using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;

namespace MiraiConsultMVC.Controllers
{
    public class PatientsController : Controller
    {
        //
        // GET: /Patients/

        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientProfile()
        {
            return View(getPatientDetailsByPatientId(6));
        }
        
        [HttpPost]
        public ActionResult PatientProfile(Profile profile)
        {
           if(ModelState.IsValid)
           {
               profile.RegistrationDate = DateTime.Now;
               profile.UserType = 2;
               var result = db.askmirai_patient_Insert_Update(profile.FirstName, profile.FirstName, profile.Email, profile.MobileNo, profile.Gender, profile.DateOfBirth, profile.CountryId, profile.StateId, profile.LocationId, profile.CityId, profile.Password, profile.Height, profile.Weight, profile.Address, profile.Pincode, profile.UserId, profile.RegistrationDate, profile.Status, profile.UserType, profile.UserName, profile.IsEmailVerified);
           }
           return RedirectToAction("PatientProfile");
        }

        [HttpGet]
        public IList<Country> poupulateCountry()
        {
           IList<Country> countryLst = new List<Country>();
            var countrylist = db.countries.ToList().OrderBy(c=>c.name);
            if(countrylist != null && countrylist.Count() > 0)
            {
                foreach(var country in countrylist)
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

        [HttpGet]
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

        [HttpGet]
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
        public IList<State> poupulateStateByCountry(int countryId)
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
        [HttpGet]
        public Profile getPatientDetailsByPatientId(int userid)
        {
            Profile patientDetail = new Profile();
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            var patient = db.users.FirstOrDefault(p => p.userid.Equals(userid));          
            if (patient != null)
            {
                patientDetail.UserId = Convert.ToInt32(patient.userid);
                patientDetail.FirstName = Convert.ToString(patient.firstname);
                patientDetail.LastName = Convert.ToString(patient.lastname);
                patientDetail.Email = Convert.ToString(patient.email);
                if (patient.mobileno != null)
                    patientDetail.MobileNo = Convert.ToString(patient.mobileno);
                if (patient.gender != null)
                    patientDetail.Gender = Convert.ToInt32(patient.gender);
                if (patient.address != null)
                    patientDetail.Address = Convert.ToString(patient.address);
                if (patient.height != null)
                    patientDetail.Height = Convert.ToInt32(patient.height);
                if (patient.weight != null)
                    patientDetail.Weight = Convert.ToDecimal(patient.weight);
                if (patient.pincode != null)
                    patientDetail.Pincode = Convert.ToInt32(patient.pincode);
                if (patient.dateofbirth != null)
                    patientDetail.DateOfBirth = Convert.ToDateTime(patient.dateofbirth);
                if (patient.countryid != null && patient.countryid != 0)
                {
                    var countryList = poupulateCountry();
                    ViewBag.countries = new SelectList(countryList,"countryid","name");
                    patientDetail.CountryId = Convert.ToInt32(patient.countryid);
                }
                if (patient.stateid != null && patient.stateid != 0)
                {
                    var stateList = poupulateState(Convert.ToInt32(patient.countryid));
                    ViewBag.states = new SelectList(stateList, "stateId", "name");
                    patientDetail.StateId = Convert.ToInt32(patient.stateid);
                }
                if (patient.cityid != null && patient.cityid != 0)
                {
                    var cityList = poupulateCity(Convert.ToInt32(patient.stateid));
                    ViewBag.cities = new SelectList(cityList, "cityId", "name");
                    patientDetail.CityId = Convert.ToInt32(patient.cityid);
                }
                if (patient.locationid != null && patient.locationid != 0)
                {
                    var locationList = poupulateLocation(Convert.ToInt32(patient.cityid));
                    ViewBag.locations = new SelectList(locationList, "locationId", "name", "cityid");
                    patientDetail.LocationId = Convert.ToInt32(patient.locationid);
                }
                if (patient.username != null)
                    patientDetail.UserName = Convert.ToString(patient.username);
                if (patient.registrationdate != null)
                    patientDetail.RegistrationDate = Convert.ToDateTime(patient.registrationdate);
                if (patient.status != null)
                    patientDetail.Status = Convert.ToInt32(patient.status);
                if (patient.isemailverified != null)
                    patientDetail.IsEmailVerified = Convert.ToBoolean(patient.isemailverified);
            }
            return patientDetail;
        }
    }
}
