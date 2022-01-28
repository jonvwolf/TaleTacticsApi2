using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record PlayerTextLogModel(
        [property: MinLength(1), MaxLength(100), Required] string Message,
        [property: MinLength(1), MaxLength(100), Required] string PlayerName);
}
