using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests3.Api.EndpointHelpers
{
    internal static class FileEndpoints
    {
        public static async Task AssertOkImageAsync(HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static async Task NotFoundImageAssert(HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        public static async Task AssertOkAudioAsync(HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static async Task NotFoundAudioAssert(HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

    }
}
