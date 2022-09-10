using HorrorTacticsApi2.Domain.Models;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Microsoft.AspNetCore.Http;
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
        const string Path = "/login";
        public LoginControllerTests(ApiTestsCollection factory)
        {
            _factory = factory;
            _factory.WebAppFactory.Options.DbName = nameof(LoginControllerTests);
        }

        [Fact]
        public async Task Should_Return_Token_With_Valid_Credentials()
        {
            // arrange
            using var client = _factory.CreateClient();
            var login = new LoginModel(_factory.WebAppFactory.MainPassword, Constants.AdminUsername);

            // act
            var response = await client.PostAsync(Path, Helper.GetContent(login));

            // assert
            var responseModel = await Helper.VerifyAndGetAsync<TokenModel>(response, StatusCodes.Status200OK);
            Assert.NotEmpty(responseModel.Token);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_With_Bad_Password()
        {
            // arrange
            using var client = _factory.CreateClient();
            var login = new LoginModel(_factory.WebAppFactory.MainPassword + "1", Constants.AdminUsername);

            // act
            var response = await client.PostAsync(Path, Helper.GetContent(login));

            // assert
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
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
