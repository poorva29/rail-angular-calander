using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MiraiConsultMVC.Models.Patients
{
    public class InviteFriend
    {
        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*([,]\\s*\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)*", ErrorMessage = "Please enter valid email address or comma separated multiple email addresses.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter email body")]
        public string message { get; set; }
    }
}