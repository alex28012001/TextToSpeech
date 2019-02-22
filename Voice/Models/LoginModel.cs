using System.ComponentModel.DataAnnotations;

namespace Voice.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "user name не может быть пустым")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "пароль не может быть пустым")]
        public string Password { get; set; }
    }
}