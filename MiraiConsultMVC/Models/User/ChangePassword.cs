using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models.User
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please Enter Current Password")]
        [StringLength(30, ErrorMessage = "Password length should be minimum 6 characters.", MinimumLength = 6)]
        public string currentPassword { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [StringLength(30, ErrorMessage = "Password length should be minimum 6 characters.", MinimumLength = 6)]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("newPassword", ErrorMessage = "Password and Confirm password should be same.")]
        
        public string confirmpassword { get; set; }
    }
}