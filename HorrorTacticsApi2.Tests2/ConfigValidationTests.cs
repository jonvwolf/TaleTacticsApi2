using HorrorTacticsApi2.Tests2.Api.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests
{
    public class ConfigValidationTests
    {
        readonly CustomWebAppFactory _factory;
        
        public ConfigValidationTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Test]
        public async Task AddOptions_Should_Validate_Complex_Objects_And_Lists()
        {
            
        }

        [Test]
        public async Task AddOptions_Should_Validate()
        {

        }
    }
}
