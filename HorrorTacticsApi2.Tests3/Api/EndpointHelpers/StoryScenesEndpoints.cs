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
    internal static class StorySceneEndpoints
    {
        public static async Task<ReadStorySceneModel> GetAsync(HttpClient client, long id)
        {
            using var response = await client.GetAsync($"secured/stories/scenes/{id}");

            return await Helper.VerifyAndGetAsync<ReadStorySceneModel>(response, StatusCodes.Status200OK);
        }

        public static async Task<List<ReadStorySceneModel>> GetAllAsync(HttpClient client, long storyId)
        {
            using var response = await client.GetAsync($"secured/stories/{storyId}/scenes");

            var dto = await Helper.VerifyAndGetAsync<List<ReadStorySceneModel>>(response, StatusCodes.Status200OK);

            return dto;
        }

        public static async Task<ReadStorySceneModel> CreateAndAssertAsync(HttpClient client, long storyId, CreateStorySceneModel model)
        {
            using var response = await client.PostAsync($"secured/stories/{storyId}/scenes", Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadStorySceneModel>(response, StatusCodes.Status201Created);

            // TODO: assert

            return dto;
        }

        public static async Task<ReadStorySceneModel> PutAndAssertAsync(HttpClient client, long id, UpdateStorySceneModel model)
        {
            using var response = await client.PutAsync($"secured/stories/scenes/{id}", Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadStorySceneModel>(response, StatusCodes.Status200OK);

            // TODO: assert

            return dto;
        }

        public static async Task DeleteAndAssertAsync(HttpClient client, long id)
        {
            {
                using var response = await client.DeleteAsync($"secured/stories/scenes/{id}");
                Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            }
            {
                using var response = await client.GetAsync($"secured/stories/scenes/{id}");
                Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
            }
        }

        public static void AssertModels(ReadStorySceneModel expected, ReadStorySceneModel received)
        {
            // TODO: assert
        }
    }
}
