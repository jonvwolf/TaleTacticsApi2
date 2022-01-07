using HorrorTacticsApi2.Tests2.Api.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests2.Api
{
    public class LoggingTests
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public LoggingTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Test]
        public async Task Should_Create_Log()
        {
            // There can only be one instance of CustomWebAppFactory with enabled logging
            // To override DisableLogging, I guess I can use an environment variable HT_GENERAL__DISABLE_LOGGING
        }

    }
}
