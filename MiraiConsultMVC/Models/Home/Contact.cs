using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MiraiConsultMVC.Models.Home
{
    public class Contact
    {
        [Required(ErrorMessage = "Please Enter Your Name.")]
        [AllowHtml]
        public string Name{get;set;}

        [Required(ErrorMessage = "Please Enter Email.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Please enter email body.")]
        public string message { get; set; }
    }
}