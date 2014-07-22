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
        public string currentPassword { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("newPassword", ErrorMessage = "'Confirm Password' and 'New Password' do not match. ")]
        public string confirmpassword { get; set; }
    }
}