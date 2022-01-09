using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class Image : IValidatableEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(ValidationConstants.Image_Name_MaxStringLength), Required]
        public string Name { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageFormatsEnum Format { get; set; }
        [StringLength(ValidationConstants.Image_AbsoluteUrl_MaxStringLength), Required]
        public string AbsolutePath { get; set; } = "";

        public void Validate()
        {
            if (Format == ImageFormatsEnum.Invalid)
                throw new InvalidOperationException($"Invalid format value: {Format}");
        }
    }
}
