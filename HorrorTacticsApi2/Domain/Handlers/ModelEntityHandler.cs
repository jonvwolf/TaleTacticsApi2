using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Domain
{
    public abstract class ModelEntityHandler
    {
        readonly IHttpContextAccessor context;
        public ModelEntityHandler(IHttpContextAccessor context)
        {
            this.context = context;
        }
        public virtual string GetUrlForFile(string filename, FileFormatEnum format)
        {
            var request = context.HttpContext?.Request;

            if (request == default)
                throw new NotImplementedException("This method should only be called within a HTTP request: " + nameof(GetUrlForFile));

            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            
            // TODO: improve this?
            string subPath = format switch
            {
                FileFormatEnum.JPG or FileFormatEnum.JPEG or FileFormatEnum.PNG => Constants.GetFileImageApiPath(filename),
                FileFormatEnum.MP3 => Constants.GetFileAudioApiPath(filename),
                _ => throw new InvalidOperationException("Unkown format or invalid value: " + format.ToString()),
            };

            var url = $"{request.Scheme}://{host}{pathBase}/{subPath}";
            return url;
        }
    }
}
