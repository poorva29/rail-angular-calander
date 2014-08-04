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
        [StringLength(6, ErrorMessage = "Password length should be minimum {1} characters.", MinimumLength = 6)]
        public string currentPassword { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [StringLength(6, ErrorMessage = "Password length should be minimum {1} characters.", MinimumLength = 6)]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("newPassword", ErrorMessage = "'Confirm Password' and 'New Password' do not match. ")]
        public string confirmpassword { get; set; }
    }
}