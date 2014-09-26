using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Model
{
    public class DoctorLocations
    {
        private int doctorlocationid;
        private int countryid;
        private string country;
        private int stateid;
        private string state;
        private int cityid;
        private string city;
        private int locationid;
        private string location;
        private string clinicname;
        private string address;
        private string telephone;
        private string hospitalname;

        public int DoctorLocationId { get { return doctorlocationid; } set { doctorlocationid = value; } }
        public int CountryId { get { return countryid; } set { countryid = value; } }
        public string Country { get { return country; } set { country = value; } }
        public int StateId { get { return stateid; } set { stateid = value; } }
        public string State { get { return state; } set { state = value; } }
        public int CityId { get { return cityid; } set { cityid = value; } }
        public string City { get { return city; } set { city = value; } }
        public int LocationId { get { return locationid; } set { locationid = value; } }
        public string Location { get { return location; } set { location = value; } }
        public string ClinicName { get { return clinicname; } set { clinicname = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string Telephone { get { return telephone; } set { telephone = value; } }
        public string HospitalName { get { return hospitalname; } set { hospitalname = value; } }
    }
}