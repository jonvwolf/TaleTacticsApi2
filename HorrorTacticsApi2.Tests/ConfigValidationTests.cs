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
    public class ConfigValidationTests : IClassFixture<CustomWebAppFactory>
    {
        readonly CustomWebAppFactory _factory;
        
        public ConfigValidationTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddOptions_Should_Validate_Complex_Objects_And_Lists()
        {
            
        }

        [Fact]
        public async Task AddOptions_Should_Validate()
        {

        }
    }
}
