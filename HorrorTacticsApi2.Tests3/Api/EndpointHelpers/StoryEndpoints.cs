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
    internal static class StoryEndpoints
    {
        public static async Task<ReadStoryModel> GetAsync(HttpClient client, long id)
        {
            using var response = await client.GetAsync($"secured/stories/{id}");

            return await Helper.VerifyAndGetAsync<ReadStoryModel>(response, StatusCodes.Status200OK);
        }

        public static async Task<List<ReadStoryModel>> GetAllAsync(HttpClient client)
        {
            using var response = await client.GetAsync("secured/stories");

            var dto = await Helper.VerifyAndGetAsync<List<ReadStoryModel>>(response, StatusCodes.Status200OK);
            
            return dto;
        }

        public static async Task<ReadStoryModel> CreateAndAssertAsync(HttpClient client, CreateStoryModel model)
        {
            using var response = await client.PostAsync("secured/stories", Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadStoryModel>(response, StatusCodes.Status201Created);

            // TODO: assert

            return dto;
        }

        public static async Task<ReadStoryModel> PutAndAssertAsync(HttpClient client, long id, UpdateStoryModel model)
        {
            using var response = await client.PutAsync($"secured/stories/{id}", Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadStoryModel>(response, StatusCodes.Status200OK);

            // TODO: assert

            return dto;
        }

        public static async Task DeleteAndAssertAsync(HttpClient client, long id)
        {
            {
                using var response = await client.DeleteAsync($"secured/stories/{id}");
                Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            }
            {
                using var response = await client.GetAsync($"secured/stories/{id}");
                Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
            }
        }

        public static void AssertModels(ReadStoryModel expected, ReadStoryModel received)
        {
            // TODO: assert
        }
    }
}
