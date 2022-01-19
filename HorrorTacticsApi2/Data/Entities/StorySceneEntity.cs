﻿using HorrorTacticsApi2.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorrorTacticsApi2.Data.Entities
{
    public class StorySceneEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public StoryEntity ParentStory { get; set; } = StoryEntity.EmptyStory;

        /// <summary>
        /// Validation length is specific (no need to share)
        /// </summary>
        [MaxLength(15000)]
        public string Texts { get; set; } = string.Empty;
        /// <summary>
        /// Validation length is specific (no need to share)
        /// </summary>
        [MaxLength(1000)]
        public string Timers { get; set; } = string.Empty;

        // TODO: make sure these are not cascade deleted
        public List<ImageEntity> Images { get; protected set; } = new();
        public List<AudioEntity> Audios { get; protected set; } = new();

        public StorySceneEntity()
        {

        }

        public StorySceneEntity(StoryEntity parent,
            string texts, string timers, 
            IReadOnlyList<ImageEntity> images, IReadOnlyList<AudioEntity> audios)
        {
            Texts = texts;
            Timers = timers;
            // TODO: be consistent, in some other models/entities are not doing this...
            Images = images.ToList();
            Audios = audios.ToList();
            ParentStory = parent;
        }
    }
}