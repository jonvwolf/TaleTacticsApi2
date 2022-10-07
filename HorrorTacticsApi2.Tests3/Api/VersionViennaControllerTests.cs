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
    public class VersionViennaControllerTests : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _factory;
        const string Path = "/VersionVienna";
        public VersionViennaControllerTests(ApiTestsCollection factory)
        {
            _factory = factory;
            _factory.WebAppFactory.Options.DbName = nameof(VersionViennaControllerTests);
        }

        [Fact]
        public async Task Should_Return_Text()
        {
            // arrange
            using var client = _factory.CreateClient();
            
            // act
            var response = await client.GetAsync(Path);
            var text = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Contains("Date: ", text);
        }

    }
}
