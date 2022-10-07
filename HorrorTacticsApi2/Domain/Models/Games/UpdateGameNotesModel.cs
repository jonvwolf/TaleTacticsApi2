using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Models.Games
{
    public record UpdateGameNotesModel(
        [MaxLength(ValidationConstants.Game_Notes_MaxStringLength),
        Required, RegularExpression(ValidationConstants.RegularExpressionForAllStrings,
        MatchTimeoutInMilliseconds = ValidationConstants.RegularExpressionTimeoutMilliseconds)]
        string Notes
    );
}
