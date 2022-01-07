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
    public class LoginControllerTests
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public LoginControllerTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Test]
        public async Task Should_Return_Token_With_Valid_Credentials()
        {

        }

        [Test]
        public async Task Should_Return_Unauthorized_With_Bad_Password()
        {

        }

        [Test]
        public async Task Should_Return_Unauthorized_With_Forged_Token()
        {
            // change in config a different token jwt key
        }

        [Test]
        public async Task TestApi_NoAuthentication_ReturnsUnauthorized()
        {
        }

        [Test]
        public async Task TestApi_InvalidToken_ReturnsUnauthorized()
        {
        }
    }
}
