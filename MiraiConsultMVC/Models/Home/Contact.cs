﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MiraiConsultMVC.Models.Home
{
    public class Contact
    {
        [Required(ErrorMessage = "Please Enter Your Name.")]
       
        public string Name{get;set;}

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter email body.")]
        public string message { get; set; }
    }
}