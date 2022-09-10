using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Users
{
    public record UpdateUserModel(
        [MaxLength(ValidationConstants.User_Password_MaxLength),
        MinLength(ValidationConstants.User_Password_MinLength)]
        string? Password)
    {
    }
}
