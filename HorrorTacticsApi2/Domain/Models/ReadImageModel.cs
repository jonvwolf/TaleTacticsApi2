using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Domain.Dtos
{
    public class ReadImageModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageFormatsEnum Format { get; set; }
        public string AbsoluteUrl { get; set; } = "";

        public ReadImageModel()
        {

        }

        public ReadImageModel(long id, string name, int w, int h, ImageFormatsEnum format, string absoluteUrl)
        {
            Id = id;
            Name = name;
            Width = w;
            Height = h;
            Format = format;
            AbsoluteUrl = absoluteUrl;
        }
    }
}
