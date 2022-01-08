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
    public class ErrorDisplayTests : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _factory;
        
        public ErrorDisplayTests(ApiTestsCollection factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_Exception_Details_In_Development_Mode()
        {
            
        }

        [Fact]
        public async Task Should_Return_InternalServerError_Instead_Of_Exception_Details_In_Production()
        {

        }

        [Fact]
        public async Task Should_Return_InternalServerError_With_Details_For_HtExceptions()
        {
            // HttpExceptionFilter
            // Inject new controllers that only throw these kind of exceptions
        }

        [Fact]
        public async Task Should_Return_BadRequest_For_HtBadRequestExceptions()
        {
            // HttpExceptionFilter
            // Inject new controllers that only throw these kind of exceptions

            // TODO: BadRequest should give the details
        }

        [Fact]
        public async Task Should_Return_NotFound_For_Route_That_Doesnt_Exist()
        {

        }

        [Fact]
        public async Task Copy_Tests_From_BSC()
        {
            // TODO: check which tests I am missing from BSC
        }
    }
}
