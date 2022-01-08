using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace HorrorTacticsApi2.Domain.Dtos
{
    public class UpdateImageModel
    {
        [MaxLength(ValidationConstants.Image_Name_MaxStringLength),
            MinLength(ValidationConstants.Image_Name_MinStringLength),
            Required]
        public string Name { get; set; } = "";
        [MinLength(1)]
        public byte[] Data { get; set; } = Array.Empty<byte>();

        public UpdateImageModel()
        {

        }

        public UpdateImageModel(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

    }
}
