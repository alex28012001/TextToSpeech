using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Voice.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "user name не может быть пустым")]
        [Remote("IsUserNameBusy", "account", ErrorMessage = "логин занят")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "пароль не может быть пустым")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "длина пароля должена быть в промежетке от 5 до 20")]
        public string Password { get; set; }
    }
}