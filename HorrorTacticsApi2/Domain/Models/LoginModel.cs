using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models
{
    public class LoginModel
    {
        [Required, MinLength(1), MaxLength(100)]
        public string Password { get; set; } = "";

        public LoginModel()
        {

        }

        public LoginModel(string pwd)
        {
            Password = pwd;
        }
    }
}
