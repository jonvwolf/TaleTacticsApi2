using HorrorTacticsApi2.Tests3.Api.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests3
{
    public class ConfigValidationTests
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
