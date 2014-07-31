﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Model;

namespace MiraiConsultMVC.Models
{
    public class DoctorProfile
    {
        private int userid;
        private string firstname;
        private string lastname;
        private string email;
        public string username; 
        private string mobileno;
        private int gender;
        private DateTime? dateofbirth = null;
        private int countryid;
        private DateTime registrationdate;
        private int status;
        private int usertype;
        private string image;
        private bool isemailverified;
        private string address;
        private string registrationnumber;
        private int registrationcouncil;
        private string aboutme;
        private bool isdocconnectuser;
        private int docconectdoctorid;
        private string photourl;
        private int specialityid;
        private string speciality;
        public IEnumerable<SelectListItem> Countries;
        public IEnumerable<SelectListItem> Registrationcouncils;
        //public IEnumerable<SelectListItem> Specialities;

        public IList<DoctorSpeciality> specialities;
        public IList<DoctorLocations> locations;
        public IList<doctorqualifications> qualification;
        public IList<doctordetails> details;
        

        public int UserId { get { return userid; } set { userid = value; } }
        public string FirstName { get { return firstname; } set { firstname = value; } }
        public string LastName { get { return lastname; } set { lastname = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string MobileNo { get { return mobileno; } set { mobileno = value; } }
        public int Gender { get { return gender; } set { gender = value; } }
        public DateTime? DateOfBirth { get { return dateofbirth; } set { this.dateofbirth = value; } }
        public string UserName { get { return username; } set { username = value; } }
        public int CountryId { get { return countryid; } set { countryid = value; } }
        public DateTime RegistrationDate { get { return registrationdate; } set { registrationdate = value; } }
        public int Status { get { return status; } set { status = value; } }
        public int UserType { get { return usertype; } set { usertype = value; } }
        public string Image { get { return image; } set { image = value; } }
        public bool IsEmailVerified { get { return isemailverified; } set { isemailverified = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string RegistrationNumber { get { return registrationnumber; } set { registrationnumber = value; } }
        public int RegistrationCouncil { get { return registrationcouncil; } set { registrationcouncil = value; } }
        public string AboutMe { get { return aboutme; } set { aboutme = value; } }
        public bool IsDocConnectUser { get { return isdocconnectuser; } set { isdocconnectuser = value; } }
        public int DocConnectDoctorId { get { return docconectdoctorid; } set { docconectdoctorid = value; } }
        public string PhotoUrl { get { return photourl; } set { photourl = value; } }
        public string Speciality { get { return speciality; } set { speciality = value; } }
        public int SpecialityId { get { return specialityid; } set { specialityid = value; } }

        public DoctorProfile()
        {
            specialities = new List<DoctorSpeciality>();
            locations = new List<DoctorLocations>();
            qualification = new List<doctorqualifications>();
            details = new List<doctordetails>();
        }
        public void AddSpeciality(DoctorSpeciality Speciality)
        {
            specialities.Add(Speciality);
        }
        public void RemoveSpeciality(DoctorSpeciality Speciality)
        {
            this.specialities.Remove(Speciality);
        }
        public void AddLocations(DoctorLocations Location)
        {
            locations.Add(Location);
        }
        public void RemoveLocations(DoctorLocations Location)
        {
            this.locations.Remove(Location);
        }
    }
}