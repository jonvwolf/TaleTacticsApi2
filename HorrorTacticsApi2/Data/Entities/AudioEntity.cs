using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class AudioEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public bool IsBgm { get; set; }
        public uint DurationSeconds { get; set; }

        [Required]
        public FileEntity File { get; set; } = FileEntity.EmptyFile;
        public List<StorySceneCommandEntity> SceneCommands { get; set; } = new();

        public AudioEntity()
        {

        }

        public AudioEntity(FileEntity file, bool isBgm, uint durationSeconds)
        {
            File = file;
            IsBgm = isBgm;
            DurationSeconds = durationSeconds;
        }
    }
}
