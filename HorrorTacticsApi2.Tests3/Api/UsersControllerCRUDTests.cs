using HorrorTacticsApi2.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Xunit;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Domain.Models.Users;
using HorrorTacticsApi2.Tests3.Api.EndpointHelpers;
using HorrorTacticsApi2.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Linq;

namespace HorrorTacticsApi2.Tests3.Api
{
    public class UsersControllerCRUDTests : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _collection;
        const string Path = "secured/users";
        public UsersControllerCRUDTests(ApiTestsCollection apiTestsCollection)
        {
            _collection = apiTestsCollection;
            _collection.WebAppFactory.Options.DbName = nameof(UsersControllerCRUDTests);
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            using var client = _collection.CreateClient();

            var created = await CreateUser(client, "test1");
            var created2 = await CreateUser(client, "test2");
            var getUser = await GetUser(client, created.Id);
            var updateUser = await UpdateUser(client, created.Id, new UpdateUserModel("mynewpasswordtest"));

            var login = new LoginModel("mynewpasswordtest", "test1");
            var response = await client.PostAsync("/login", Helper.GetContent(login));
            var responseModel = await Helper.VerifyAndGetAsync<TokenModel>(response, StatusCodes.Status200OK);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, responseModel.Token);

            var stories = await StoryEndpoints.GetAllAsync(client);
            Assert.NotEmpty(stories);
            var story = stories.FirstOrDefault();
            Assert.NotNull(story);
            Assert.Equal("A father's legacy (Example story)", story.Title);

            var newStory = await StoryEndpoints.CreateAndAssertAsync(client, new CreateStoryModel("My new title", "my new desc"));
            var storyModel = await StoryEndpoints.GetAsync(client, newStory.Id);
            Assert.NotNull(storyModel);
            Assert.Equal("My new title", storyModel.Title);
            await ValidateOtherUser(client, story);

            Assert.Equal(created.Username, getUser.Username);
            Assert.Equal(created.Role, getUser.Role);
        }

        static async Task ValidateOtherUser(HttpClient client, ReadStoryModel existingStory)
        {
            var login = new LoginModel("passwordtest", "test2");
            var response = await client.PostAsync("/login", Helper.GetContent(login));
            var responseModel = await Helper.VerifyAndGetAsync<TokenModel>(response, StatusCodes.Status200OK);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, responseModel.Token);

            await StoryEndpoints.NotFoundAssert(client, existingStory.Id);

            var stories = await StoryEndpoints.GetAllAsync(client);
            Assert.Single(stories);
            Assert.Equal("A father's legacy (Example story)", stories.First().Title);

            var storyCommandWithCommentsAndInternalTimer = stories[0].StoryScenes
                .Select(x => x.StorySceneCommands.Where(x => x.Comments != default && x.StartInternalTimer).FirstOrDefault())
                .FirstOrDefault();

            Assert.NotNull(storyCommandWithCommentsAndInternalTimer);
            Assert.Contains("This button also starts an internal timer", storyCommandWithCommentsAndInternalTimer.Comments);

            var storyCommandWithTimer = stories[0].StoryScenes.FirstOrDefault(x => x.StorySceneCommands.Any(x => x.Timers.Count > 0 && x.Timers[0] == 60));

            Assert.NotNull(storyCommandWithTimer);

            var imageModel = stories.First().StoryScenes.First().StorySceneCommands.First().Images.First();
            var imageModelToCheck = existingStory.StoryScenes.First().StorySceneCommands.First().Images.First();

            var audioModel = stories.First().StoryScenes.First().StorySceneCommands.First().Audios.First();
            var audioModelToCheck = existingStory.StoryScenes.First().StorySceneCommands.First().Audios.First();

            await FileEndpoints.AssertOkImageAsync(client, imageModel.AbsoluteUrl);
            await ImageEndpoints.DeleteAndAssertAsync(client, imageModel.Id);
            await FileEndpoints.NotFoundImageAssert(client, imageModel.AbsoluteUrl);
            await FileEndpoints.AssertOkImageAsync(client, imageModelToCheck.AbsoluteUrl);

            await FileEndpoints.AssertOkAudioAsync(client, audioModel.AbsoluteUrl);
            await AudioEndpoints.DeleteAndAssertAsync(client, audioModel.Id);
            await FileEndpoints.NotFoundAudioAssert(client, audioModel.AbsoluteUrl);
            await FileEndpoints.AssertOkAudioAsync(client, audioModelToCheck.AbsoluteUrl);
        }

        static async Task<ReadUserModel> CreateUser(HttpClient client, string username)
        {
            using var response = await client.PostAsync(Path, Helper.GetContent(new CreateUserModel(username, "passwordtest")));

            var dto = await Helper.VerifyAndGetAsync<ReadUserModel>(response, StatusCodes.Status201Created);

            return dto;
        }

        static async Task<ReadUserModel> GetUser(HttpClient client, long id)
        {
            using var response = await client.GetAsync(Path + "/" + id);

            var dto = await Helper.VerifyAndGetAsync<ReadUserModel>(response, StatusCodes.Status200OK);

            return dto;
        }

        static async Task<ReadUserModel> UpdateUser(HttpClient client, long id, UpdateUserModel model)
        {
            using var response = await client.PutAsync(Path + "/" + id, Helper.GetContent(model));

            var dto = await Helper.VerifyAndGetAsync<ReadUserModel>(response, StatusCodes.Status200OK);

            return dto;
        }

    }
}
