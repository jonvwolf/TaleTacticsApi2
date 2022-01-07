using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests.Api.Helpers
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
            _webAppFactory = new CustomWebAppFactory();

            if (!File.Exists(Constants.FILE_APPLY_MIGRATIONS))
            {
                File.WriteAllText(Constants.FILE_APPLY_MIGRATIONS, "");
                File.SetAttributes(Constants.FILE_APPLY_MIGRATIONS, FileAttributes.ReadOnly);
            }
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
