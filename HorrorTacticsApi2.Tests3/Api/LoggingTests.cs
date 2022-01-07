using HorrorTacticsApi2.Tests3.Api.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests3.Api
{
    public class LoggingTests : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _factory;
        const string Path = "api/images";
        public LoggingTests(ApiTestsCollection factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Create_Log()
        {
            // There can only be one instance of CustomWebAppFactory with enabled logging
            // To override DisableLogging, I guess I can use an environment variable HT_GENERAL__DISABLE_LOGGING
        }

    }
}
