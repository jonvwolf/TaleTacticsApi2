using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models
{
    public class LoginModel
    {
        [Required, MinLength(1), MaxLength(100)]
        public string Password { get; set; } = "";

        [Required, MinLength(1), MaxLength(100)]
        public string Username { get; set; } = "";

        public LoginModel()
        {

        }

        public LoginModel(string pwd, string username)
        {
            Password = pwd;
            Username = username;
        }
    }
}
