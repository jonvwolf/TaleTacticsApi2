using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Dtos
{
    public record UpdateImageModel(
            [MaxLength(ValidationConstants.File_Name_MaxStringLength),
            MinLength(ValidationConstants.File_Name_MinStringLength),
            Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)] string Name);
}
