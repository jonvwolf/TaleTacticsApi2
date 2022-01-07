using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;

namespace HorrorTacticsApi2.Domain
{
    public class ImageModelEntityConverter
    {
        public Image CreateEntity(CreateImageModel model)
        {
            // TODO: convert byte into file, and get w,h and format
            return new Image()
            {
                Name = model.Name,
                Width = 1,
                Height = 2,
                AbsolutePath = "path",
                Format = Common.FormatsEnum.PNG
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
