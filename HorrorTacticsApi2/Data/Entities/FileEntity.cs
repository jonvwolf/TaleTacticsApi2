using HorrorTacticsApi2.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class FileEntity : IValidatableEntity
    {
        public static readonly FileEntity EmptyFile = new();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(ValidationConstants.File_Name_MaxStringLength, MinimumLength = ValidationConstants.File_Name_MinStringLength), Required]
        public string Name { get; set; } = string.Empty;
        public FileFormatEnum Format { get; set; }
        [StringLength(ValidationConstants.File_Filename_MaxStringLength, MinimumLength = ValidationConstants.File_Name_MinStringLength), Required]
        public string Filename { get; set; } = string.Empty;
        public long SizeInBytes { get; set; }
        public bool IsVirusScanned { get; set; }
        [Required]
        public UserEntity Owner { get; set; } = UserEntity.EmptyUser;
        public bool IsDefault { get; set; }
        public FileEntity()
        {

        }

        public FileEntity(string name, FileFormatEnum format, string filename, long size, UserEntity owner)
        {
            Name = name;
            Format = format;
            Filename = filename;
            SizeInBytes = size;
            Owner = owner;
        }

        public void Validate()
        {
            if (Format == FileFormatEnum.Invalid)
                throw new InvalidOperationException($"Invalid format value: {Format}");
        }
    }
}
