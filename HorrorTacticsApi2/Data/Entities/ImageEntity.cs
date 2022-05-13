using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class ImageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        [Required]
        public FileEntity File { get; set; } = FileEntity.EmptyFile;
        public List<StorySceneCommandEntity> SceneCommands { get; set; } = new();

        public ImageEntity()
        {

        }

        public ImageEntity(FileEntity file, uint w, uint h)
        {
            Width = w;
            Height = h;
            File = file;
        }
    }
}
