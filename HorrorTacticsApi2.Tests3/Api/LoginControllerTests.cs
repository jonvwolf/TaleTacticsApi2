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
    public class LoginControllerTests : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _factory;
        const string Path = "api/images";
        public LoginControllerTests(ApiTestsCollection factory)
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

        [Fact]
        public async Task TestApi_NoAuthentication_ReturnsUnauthorized()
        {
        }

        [Fact]
        public async Task TestApi_InvalidToken_ReturnsUnauthorized()
        {
        }
    }
}
