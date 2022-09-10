using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Users
{
    public record CreateUserModel(
        [MaxLength(ValidationConstants.User_Username_MaxLength),
        MinLength(ValidationConstants.User_Username_MinLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Username,
        [MaxLength(ValidationConstants.User_Password_MaxLength),
        MinLength(ValidationConstants.User_Password_MinLength),
        Required]
        string Password)
    {
    }
}
