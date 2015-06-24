using System.ComponentModel.DataAnnotations;

namespace MiraiConsultMVC.Models.User
{
    public class Login
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool IsCampainUser { get; set; }
        public int QuestionId { get; set; }
        public bool IsUserRegistered { get; set; }
    }
}