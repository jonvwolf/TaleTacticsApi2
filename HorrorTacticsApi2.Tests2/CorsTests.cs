using HorrorTacticsApi2.Tests2.Api.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests2
{
    public class CorsTests
    {
        readonly CustomWebAppFactory _factory;
        
        public CorsTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Test]
        public async Task Should_Only_Allow_Admin_Endpoints_From_Specific_Host()
        {
            // host value is in config
        }

        [Test]
        public async Task Should_Allow_From_All_For_Game_Endpoints()
        {

        }
    }
}
