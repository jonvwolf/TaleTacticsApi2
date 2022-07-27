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
    internal static class StorySceneCommandsEndpoints
    {
        public static async Task<ReadStorySceneCommandModel> GetAsync(HttpClient client, long id)
        {
            using var response = await client.GetAsync($"secured/scenes/commands/{id}");

            return await Helper.VerifyAndGetAsync<ReadStorySceneCommandModel>(response, StatusCodes.Status200OK);
        }

        public static async Task<List<ReadStorySceneCommandModel>> GetAllAsync(HttpClient client, long storySceneId)
        {
            using var response = await client.GetAsync($"secured/scenes/{storySceneId}/commands");

            var dto = await Helper.VerifyAndGetAsync<List<ReadStorySceneCommandModel>>(response, StatusCodes.Status200OK);

            return dto;
        }

        public static async Task<ReadStorySceneCommandModel> CreateAndAssertAsync(HttpClient client, long storySceneId, CreateStorySceneCommandModel model)
        {
            using var response = await client.PostAsync($"secured/scenes/{storySceneId}/commands", Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadStorySceneCommandModel>(response, StatusCodes.Status201Created);

            // TODO: assert

            return dto;
        }

        public static async Task<ReadStorySceneCommandModel> PutAndAssertAsync(HttpClient client, long id, UpdateStorySceneCommandModel model)
        {
            using var response = await client.PutAsync($"secured/scenes/commands/{id}", Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadStorySceneCommandModel>(response, StatusCodes.Status200OK);

            // TODO: assert

            return dto;
        }

        public static async Task DeleteAndAssertAsync(HttpClient client, long id)
        {
            {
                using var response = await client.DeleteAsync($"secured/scenes/commands/{id}");
                Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            }
            {
                using var response = await client.GetAsync($"secured/scenes/commands/{id}");
                Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
            }
        }

        public static void AssertModels(ReadStorySceneCommandModel expected, ReadStorySceneCommandModel received)
        {
            // TODO: assert
        }
    }
}
