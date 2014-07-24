﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace MiraiConsultMVC.Models
{
    public class ModelUser
    {
        private int userid;
        private string firstname;
        private string lastname;
        private string email;
        private string mobileno;
        private int gender;
        private DateTime? dateofbirth = null;
        private string username;
        private string password;
        private string confirmPassword;
        private int countryid;
        private int stateid;
        private int cityid;
        private int locationid;
        private int height;
        private decimal weight;
        private DateTime registrationdate;
        private int status;
        private int usertype;
        private string image;
        private bool isemailverified;
        private string address;
        private int pincode;
        private string registrationnumber;
        private int registrationcouncil;
        private string aboutme;
        private bool isdocconnectuser;
        private int docconectdoctorid;
        private string photourl;
        public IList<DoctorSpecialities> specialities;
        public IList<DoctorLocations> locations;
        public IList<doctorqualifications> qualification;
        public IList<doctordetails> details;
        public IEnumerable<SelectListItem> Countries;

        public int UserId { get { return userid; } set { userid = value; } }

        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get { return firstname; } set { firstname = value; } }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get { return lastname; } set { lastname = value; } }

        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email address")]
        public string Email { get { return email; } set { email = value; } }

        [RegularExpression(@"^[0-9]{10}", ErrorMessage = "Please enter 10 digit mobile number.")]
        public string MobileNo { get { return mobileno; } set { mobileno = value; } }

        public int Gender { get { return gender; } set { gender = value; } }
        public DateTime? DateOfBirth { get { return dateofbirth; } set { this.dateofbirth = value; } }
        public string UserName { get { return username; } set { username = value; } }

        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(6, ErrorMessage = "Password length should be minimum {1} characters.", MinimumLength = 6)]
        public string Password { get { return password; } set { password = value; } }
        
        [Required(ErrorMessage="Please Enter Confirm Password")]
        public string ConfirmPassword { get { return confirmPassword; } set { confirmPassword = value; } }

        public int CountryId { get { return countryid; } set { countryid = value; } }
        public int StateId { get { return stateid; } set { stateid = value; } }
        public int CityId { get { return cityid; } set { cityid = value; } }
        public int LocationId { get { return locationid; } set { locationid = value; } }

        [RegularExpression(@"^[0-9]{1,6}$", ErrorMessage = "Please Enter Numbers Only.")]
        public int Height { get { return height; } set { height = value; } }

        [RegularExpression(@"(\+)?[0-9]+(\.[0-9]*)?", ErrorMessage = "Please Enter Weight In Positive Numbers.")]
        public decimal Weight { get { return weight; } set { weight = value; } }

        public DateTime RegistrationDate { get { return registrationdate; } set { registrationdate = value; } }
        public int Status { get { return status; } set { status = value; } }
        public int UserType { get { return usertype; } set { usertype = value; } }
        public string Image { get { return image; } set { image = value; } }
        public bool IsEmailVerified { get { return isemailverified; } set { isemailverified = value; } }
        public string Address { get { return address; } set { address = value; } }
        
        [RegularExpression(@"^[0-9]{10}", ErrorMessage = "Please Enter 6 Digit Pin Code Number.")]
        public int Pincode { get { return pincode; } set { pincode = value; } }
        public string RegistrationNumber { get { return registrationnumber; } set { registrationnumber = value; } }
        public int RegistrationCouncil { get { return registrationcouncil; } set { registrationcouncil = value; } }
        public string AboutMe { get { return aboutme; } set { aboutme = value; } }
        public bool IsDocConnectUser { get { return isdocconnectuser; } set { isdocconnectuser = value; } }
        public int DocConnectDoctorId { get { return docconectdoctorid; } set { docconectdoctorid = value; } }
        public string PhotoUrl { get { return photourl; } set { photourl = value; } }


        public ModelUser()
        {
            specialities = new List<DoctorSpecialities>();
            locations = new List<DoctorLocations>();
            qualification = new List<doctorqualifications>();
            details = new List<doctordetails>();
            
        }
        public void AddSpeciality(DoctorSpecialities Speciality)
        {
            specialities.Add(Speciality);
        }
        public void RemoveSpeciality(DoctorSpecialities Speciality)
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