﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MiraiConsultMVC.Models.Patients
{
    public class InviteFriend
    {
        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*([,]\\s*\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)*", ErrorMessage = "Please enter valid email address or comma separated multiple email addresses.")]
        public string email { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Please enter email body")]
        public string message { get; set; }
    }
}