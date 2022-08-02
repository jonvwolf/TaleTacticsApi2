using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Domain
{
    /// <summary>
    /// The only experts that knows how to validate and create entities and models
    /// </summary>
    public class ImageModelEntityHandler : ModelEntityHandler
    {
        public ImageModelEntityHandler(IHttpContextAccessor context) : base(context)
        {
        }
        public void Validate(UpdateImageModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public ImageEntity CreateEntity(FileUploaded file)
        {
            var htFile = new FileEntity(file.Name, file.Format, file.Filename, file.SizeInBytes);

            return new ImageEntity(htFile, 0, 0);
        }

        public void UpdateEntity(uint w, uint h, ImageEntity entity)
        {
            // This is done after scanning file for viruses
            entity.Width = w;
            entity.Height = h;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="newFile"></param>
        /// <returns>Returns the file to be deleted</returns>
        public string UpdateEntity(ImageEntity entity, FileUploaded newFile)
        {
            string oldFile = entity.File.Filename;

            entity.File.Format = newFile.Format;
            entity.File.Filename = newFile.Filename;
            entity.File.IsVirusScanned = false;
            entity.File.SizeInBytes = newFile.SizeInBytes;

            // TODO: update width/height AFTER virus scan
            return oldFile;
        }

        public void UpdateEntity(UpdateImageModel model, ImageEntity entity)
        {
            entity.File.Name = model.Name;
        }

        public ReadImageModel CreateReadModel(ImageEntity entity)
        {
            string absoluteUrl = GetUrlForFile(entity.File.Filename, entity.File.Format);
            return new ReadImageModel(
                entity.Id, 
                entity.File.Name, 
                entity.Width, 
                entity.Height, 
                entity.File.Format, 
                absoluteUrl,
                entity.File.IsVirusScanned,
                entity.File.SizeInBytes);
        }



    }
}
