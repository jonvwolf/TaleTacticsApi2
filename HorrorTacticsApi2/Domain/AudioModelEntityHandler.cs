using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Audio;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Domain
{
    /// <summary>
    /// The only experts that knows how to validate and create entities and models
    /// </summary>
    public class AudioModelEntityHandler : ModelEntityHandler
    {
        public AudioModelEntityHandler(IHttpContextAccessor context) : base(context)
        {
        }
        public void Validate(UpdateAudioModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public AudioEntity CreateEntity(FileUploaded file)
        {
            var htFile = new FileEntity(file.Name, file.Format, file.Filename, file.SizeInBytes);

            // TODO: later update this values
            return new AudioEntity(htFile, false, 0);
        }

        public void UpdateEntity(AudioEntity entity, bool? isBgm = default, uint? duration = default)
        {
            if (isBgm.HasValue)
                entity.IsBgm = isBgm.Value;

            if (duration.HasValue)
                entity.DurationSeconds = duration.Value;
        }

        public void UpdateEntity(UpdateAudioModel model, AudioEntity entity)
        {
            entity.File.Name = model.Name;
        }

        public ReadAudioModel CreateReadModel(AudioEntity entity)
        {
            string absoluteUrl = GetUrlForFile(entity.File.Filename, entity.File.Format);
            return new ReadAudioModel(
                entity.Id,
                entity.File.Name,
                entity.IsBgm,
                entity.DurationSeconds,
                entity.File.Format,
                absoluteUrl,
                entity.File.IsVirusScanned);
        }


    }
}
