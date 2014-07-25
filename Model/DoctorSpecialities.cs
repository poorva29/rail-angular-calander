using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class DoctorSpeciality
    {
        private int specialityid;
        private string speciality;
        // constructor
        public DoctorSpeciality()
        {
        }

        public string Speciality { get { return speciality; } set { speciality = value; } }
        public int SpecialityId { get { return specialityid; } set { specialityid = value; } }
    }
}