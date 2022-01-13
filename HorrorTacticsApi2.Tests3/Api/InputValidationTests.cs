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
    public class InputValidationTests : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _factory;
        const string Path = "api/images";
        public InputValidationTests(ApiTestsCollection factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_BadRequest_Invalid_Complex_Object()
        {
            // for all controllers?

            // Validate BadRequest... it should have the property name and its message, etc.
        }

        [Fact]
        public async Task Should_Return_BadRequest_Invalid_List_Complex_Object()
        {
            // for all controllers?
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Sending_Empty_Body()
        {
            // check [FromBody] model cannot be null
        }
    }
}
