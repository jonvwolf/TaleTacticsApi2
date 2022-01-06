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

namespace HorrorTacticsApi2.Tests.Api
{
    public class LoginControllerTests : IClassFixture<CustomWebAppFactory>
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public LoginControllerTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_Token_With_Valid_Credentials()
        {
            
        }

        [Fact]
        public async Task Should_Return_Unauthorized_With_Bad_Password()
        {

        }

        [Fact]
        public async Task Should_Return_Unauthorized_With_Forged_Token()
        {
            // change in config a different token jwt key
        }
    }
}
