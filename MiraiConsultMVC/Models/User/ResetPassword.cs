using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models.User
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "'Password' and 'New Password' do not match. ")]
        public string confirmpassword { get; set; }
    }
}