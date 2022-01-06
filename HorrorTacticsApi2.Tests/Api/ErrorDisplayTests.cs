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
    public class ErrorDisplayTests : IClassFixture<CustomWebAppFactory>
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public ErrorDisplayTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_Exception_Details()
        {
            
        }

        [Fact]
        public async Task Should_Return_500_Error_Instead_Of_Exception_Details()
        {

        }
    }
}
