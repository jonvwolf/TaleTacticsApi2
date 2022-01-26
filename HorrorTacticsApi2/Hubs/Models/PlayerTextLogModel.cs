using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Hubs.Models
{
    public record PlayerTextLogModel(
        [MinLength(1), MaxLength(100), Required] string Message,
        [MinLength(1), MaxLength(100), Required] string PlayerName);
}
