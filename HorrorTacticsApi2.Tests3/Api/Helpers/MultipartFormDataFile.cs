using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests3.Api
{
    internal class MultipartFormDataFile : IDisposable
    {
        private bool disposedValue;

        public MultipartFormDataContent MultipartFormDataContent { get; }
        readonly MemoryStream memoryStream;
        readonly StreamContent streamContent;

        public MultipartFormDataFile(byte[] fileContents, string filename)
        {
            MultipartFormDataContent = new MultipartFormDataContent();
            memoryStream = new MemoryStream(fileContents);
            streamContent = new StreamContent(memoryStream);

            // streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = filename
            };
            MultipartFormDataContent.Add(streamContent);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    streamContent.Dispose();
                    memoryStream.Dispose();
                    MultipartFormDataContent.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
