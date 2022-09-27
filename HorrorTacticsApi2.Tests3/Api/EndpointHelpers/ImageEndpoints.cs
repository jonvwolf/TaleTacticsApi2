using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests3.Api.EndpointHelpers
{
    internal static class ImageEndpoints
    {
        public static async Task<ReadImageModel> GetAsync(HttpClient client, long id)
        {
            using var response = await client.GetAsync($"secured/images/{id}");

            return await Helper.VerifyAndGetAsync<ReadImageModel>(response, StatusCodes.Status200OK);
        }

        public static async Task DeleteAndAssertAsync(HttpClient client, long id)
        {
            {
                using var response = await client.DeleteAsync($"secured/images/{id}");
                Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            }
            {
                using var response = await client.GetAsync($"secured/images/{id}");
                Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
            }
        }

        public static async Task NotFoundAssert(HttpClient client, long id)
        {
            using var response = await client.GetAsync($"secured/images/{id}");
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        public static void AssertModels(ReadImageModel expected, ReadImageModel received)
        {
            // TODO: assert
        }
    }
}
