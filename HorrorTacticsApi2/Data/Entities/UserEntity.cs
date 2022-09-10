using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class UserEntity
    {
        public static readonly UserEntity EmptyUser = new();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public byte[] Password { get; set; } = Array.Empty<byte>();

        public DateTime LastLogin { get; set; }

        public UserRole Role { get; set; } = UserRole.NotSet;
        [Required]
        public byte[] Salt { get; set; } = Array.Empty<byte>();
        public UserEntity()
        {

        }

        public UserEntity(string username, byte[] password, byte[] salt)
        {
            UserName = username;
            Password = password;
            Salt = salt;
        }
    }
}
