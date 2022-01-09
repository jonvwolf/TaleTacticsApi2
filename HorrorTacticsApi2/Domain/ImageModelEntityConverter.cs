using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;

namespace HorrorTacticsApi2.Domain
{
    /// <summary>
    /// The only experts that knows how to validate and create entities and models
    /// </summary>
    public class ImageModelEntityConverter
    {
        public void Validate(CreateImageModel model, bool basicValidated)
        {
            // basicValidated = true means that its data validation attributes were validated
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public void Validate(UpdateImageModel model, bool basicValidated)
        {
            if (!basicValidated)
                throw new NotImplementedException("basicValidated");

            // Just an example
            if (model == null)
                throw new HtBadRequestException("Model is null");
        }

        public Image CreateEntity(CreateImageModel model)
        {
            // TODO: convert byte into file, and get w,h and format
            return new Image()
            {
                Name = model.Name,
                Width = 1,
                Height = 2,
                AbsolutePath = "path",
                Format = ImageFormatsEnum.PNG
            };
        }

        public void SetEntity(UpdateImageModel model, Image entity)
        {
            // TODO: get the other properties
            entity.Name = model.Name;
        }

        public ReadImageModel CreateModel(Image entity)
        {
            return new ReadImageModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Width = entity.Width,
                Height = entity.Height,
                Format = entity.Format,
                // TODO: change this to an url
                AbsoluteUrl = entity.AbsolutePath
            };
        }



    }
}
