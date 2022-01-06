using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Tests.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests
{
    public class CorsTests : IClassFixture<CustomWebAppFactory>
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public CorsTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Only_Allow_Admin_Endpoints_From_Specific_Host()
        {
            // host value is in config
        }

        [Fact]
        public async Task Should_Allow_From_All_For_Game_Endpoints()
        {

        }
    }
}
