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
    public class InputValidationTests
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public InputValidationTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Test]
        public async Task Should_Return_BadRequest_Invalid_Complex_Object()
        {
            // for all controllers?
        }

        [Test]
        public async Task Should_Return_BadRequest_Invalid_List_Complex_Object()
        {
            // for all controllers?
        }

        [Test]
        public async Task Should_Return_BadRequest_When_Sending_Empty_Body()
        {
            // check [FromBody] model cannot be null
        }
    }
}
