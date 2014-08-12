using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models.User
{
    public class ResetPassword
    {
        [StringLength(30, ErrorMessage = "Password length should be minimum 6 characters.", MinimumLength = 6)]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password should be same.")]
        public string confirmpassword { get; set; }
    }
}