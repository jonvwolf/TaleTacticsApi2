using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class Image : IValidatable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(300), Required]
        public string Name { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
        public FormatsEnum Format { get; set; }
        [StringLength(5000), Required]
        public string AbsoluteUrl { get; set; } = "";

        public void Validate()
        {
            if (Format == FormatsEnum.Invalid)
                throw new ArgumentException($"Has invalid as value", nameof(Format));
        }
    }
}
