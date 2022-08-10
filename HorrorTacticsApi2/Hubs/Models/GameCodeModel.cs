using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record GameCodeModel(
        [property: MinLength(1), MaxLength(50), Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings)]  string GameCode);
}
