using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models
{
    public class Report
    {
        public int userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobileno { get; set; }
        public string city { get; set; }
        public string location { get; set; }
        public int appointmentbooked { get; set; }
        public int appointmentclicked { get; set; }
        public string avgresponsetime { get; set; }
    }
}