﻿namespace MiraiConsultMVC.Models
{
    public class doctorqualifications
    {
        private int docqualificationid;
        private string degree;
        private int degreeid;
        private int userid;
        private string university;
        // constructor
        public doctorqualifications()
        {
        }
        public int DocQualificationId { get { return docqualificationid; } set { docqualificationid = value; } }
        public string Degree { get { return degree; } set { degree = value; } }
        public int DegreeId { get { return degreeid; } set { degreeid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public string University { get { return university; } set { university = value; } }
    }
}