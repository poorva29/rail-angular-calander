using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class doctordetails
    {
        private int docdetailsid;
        private int userid;
        private string certification;
        private string society;        
         // constructor
        public doctordetails()
        {
        }
        public int DocDetailsId { get { return docdetailsid; } set { docdetailsid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public string Certification { get { return certification; } set { certification = value; } }
        public string Society { get { return society; } set { society = value; } }
    }
}