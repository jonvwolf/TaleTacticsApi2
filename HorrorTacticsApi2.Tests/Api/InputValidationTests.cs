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
    public class InputValidationTests : IClassFixture<CustomWebAppFactory>
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public InputValidationTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_BadRequest_Invalid_Complex_Object()
        {
            // for all controllers?
        }

        [Fact]
        public async Task Should_Return_BadRequest_Invalid_List_Complex_Object()
        {
            // for all controllers?
        }
    }
}
