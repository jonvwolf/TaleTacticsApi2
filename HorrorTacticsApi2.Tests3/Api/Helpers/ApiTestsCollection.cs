using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    public class ApiTestsCollection : IDisposable
    {
        /// <summary>
        /// This is to be shared
        /// </summary>
        readonly CustomWebAppFactory _webAppFactory;
        private bool disposedValue;

        public ApiTestsCollection()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "ApiTesting");
            _webAppFactory = new CustomWebAppFactory();
        }

        public HttpClient CreateClient()
        {
            return _webAppFactory.CreateClient();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _webAppFactory.Dispose();
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
