using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Xunit;
using HorrorTacticsApi2.Domain.Models.Stories;
using HorrorTacticsApi2.Domain.Models.Games;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using HorrorTacticsApi2.Hubs.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System;
using HorrorTacticsApi2.Domain.Models.Minigames;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Models.Audio;

namespace HorrorTacticsApi2.Tests3.Api
{
    public class StoriesControllerCRUDTests : IClassFixture<ApiTestsCollection>, IDisposable
    {
        readonly ApiTestsCollection _collection;
        const string Path = "secured/stories";
        const string PathStoryScenes = "secured/storyscenes";
        const string PathStorySceneCommands = "secured/storyscenes/storyscenecommands";
        const string PathGames = "games";
        
        bool Hub_Hm_Received_Player_Log_Logged_In;
        bool Hub_Hm_Received_Player_Log_Image;
        bool Hub_Hm_Received_Player_Log_Audio;
        bool Hub_Hm_Received_Player_Log_Text;
        bool Hub_Hm_Received_Player_Log_Minigame;
        private bool disposedValue;
        readonly SemaphoreSlim semaphore = new(1, 1);

        public StoriesControllerCRUDTests(ApiTestsCollection apiTestsCollection)
        {
            _collection = apiTestsCollection;
            _collection.WebAppFactory.Options.DbName = nameof(StoriesControllerCRUDTests);
        }

        public async Task<ReadStoryModel> Test2(HttpClient client)
        {
            var imageDto = await ImagesControllerCRUDTests.Post_Should_Create_Image(client, "imagex");
            var imageDto2 = await ImagesControllerCRUDTests.Post_Should_Create_Image(client, "imagex2");

            var audioDto = await AudioControllerCRUDTests.Post_Should_Create_Audio(client, "audio");
            var audioDto2 = await AudioControllerCRUDTests.Post_Should_Create_Audio(client, "audio2");

            var storyDto = await EndpointHelpers.StoryEndpoints.CreateAndAssertAsync(client, new CreateStoryModel("story title", "description x"));
            var updatedStoryDto = await EndpointHelpers.StoryEndpoints.PutAndAssertAsync(client, storyDto.Id, new UpdateStoryModel("updated x", "updated desc"));

            var expectedStory1 = new ReadStoryModel(storyDto.Id, "updated x", "updated desc", new List<ReadStorySceneModel>());

            var receivedStoryDto1 = await EndpointHelpers.StoryEndpoints.GetAsync(client, storyDto.Id);
            EndpointHelpers.StoryEndpoints.AssertModels(expectedStory1, receivedStoryDto1);

            var storySceneDto = await EndpointHelpers.StorySceneEndpoints.CreateAndAssertAsync(client, storyDto.Id, new CreateStorySceneModel("scene title"));
            var updatedStorySceneDto = await EndpointHelpers.StorySceneEndpoints.PutAndAssertAsync(client, storySceneDto.Id, new UpdateStorySceneModel("updated title"));

            var expectedStoryScene1 = new ReadStorySceneModel(storySceneDto.Id, "updated title", new List<ReadStorySceneCommandModel>());
            var expectedStory2 = expectedStory1 with
            {
                StoryScenes = new List<ReadStorySceneModel>() { expectedStoryScene1 }
            };

            var receivedStoryDto2 = await EndpointHelpers.StoryEndpoints.GetAsync(client, storyDto.Id);
            EndpointHelpers.StoryEndpoints.AssertModels(expectedStory2, receivedStoryDto2);

            var storySceneCommandDto = await EndpointHelpers.StorySceneCommandsEndpoints.CreateAndAssertAsync(client, storySceneDto.Id,
                new CreateStorySceneCommandModel(
                    "command 1",
                    new List<string>() { "Text ñ", "Hola ñ" },
                    Minigames: new List<long>() { 1 },
                    Timers: default,
                    new List<long>() { imageDto.Id },
                    new List<long>() { audioDto.Id }
                )
            );

            var updatedStorySceneCommandDto = await EndpointHelpers.StorySceneCommandsEndpoints.PutAndAssertAsync(client, storySceneCommandDto.Id,
                new UpdateStorySceneCommandModel(
                    "Updated title ú",
                    new List<string>() { "Text ñ", "Hola ñ", "ñ ñ" },
                    Minigames: default,
                    Timers: new List<uint>() { 10 },
                    new List<long>() { imageDto.Id, imageDto2.Id },
                    new List<long>() { audioDto2.Id }
                )
            );

            var expectedStorySceneCommand1 = new ReadStorySceneCommandModel(
                updatedStorySceneCommandDto.Id,
                "Updated title ú",
                new List<string>() { "Text ñ", "Hola ñ", "ñ ñ" },
                Timers: new List<uint>() { 10 },
                new List<ReadImageModel>() { imageDto, imageDto2 },
                new List<ReadAudioModel>() { audioDto2 },
                Minigames: new List<ReadMinigameModel>() { new ReadMinigameModel(1, "OK?") }
            );

            var expectedStoryScene2 = expectedStoryScene1 with
            {
                StorySceneCommands = new List<ReadStorySceneCommandModel>() { expectedStorySceneCommand1 }
            };
            var expectedStory3 = expectedStory1 with
            {
                StoryScenes = new List<ReadStorySceneModel>() { expectedStoryScene2 }
            };

            var receivedStoryDto3 = await EndpointHelpers.StoryEndpoints.GetAsync(client, storyDto.Id);
            EndpointHelpers.StoryEndpoints.AssertModels(expectedStory3, receivedStoryDto3);

            return receivedStoryDto3;
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            using var client = _collection.CreateClient();
            var storyDto = await Test2(client);

            var createdGame = await Post_Should_Create_Game(client, storyDto.Id);
            // TODO: move this to its own test collection
            var gameConfig = await Games_Get_Should_Return_GameConfiguration(client, createdGame.GameCode);
            await Get_Audio_Should_Return_File(client, gameConfig);
            await Get_Image_Should_Return_File(client, gameConfig);

            // TODO: move this to its own test collection (HUB TESTING)
            await using var hmHub = await TestGameHubAsHmAsync_Part1(_collection.GetServer(), createdGame.GameCode, _collection.WebAppFactory.Token);
            await using var hub = await TestGameHubAsPlayerAsync_Part1(_collection.GetServer(), createdGame.GameCode);
            await TestGameHubAsHm_NoToken_Exception_Async(_collection.GetServer(), createdGame.GameCode);
            TestGameHubAsPlayer_Part2(hub, createdGame.GameCode);

            if (!await semaphore.WaitAsync(1000))
                throw new Exception("Semaphore timed out1");

            await TestGameHubAsHmAsync_Part2(hmHub, createdGame.GameCode);

            if (!await semaphore.WaitAsync(1000))
                throw new Exception("Semaphore timed out2");

            await hub.StopAsync();
            await hmHub.StopAsync();

            Assert.True(Hub_Hm_Received_Player_Log_Logged_In);
            Assert.True(Hub_Hm_Received_Player_Log_Image);
            Assert.True(Hub_Hm_Received_Player_Log_Audio);
            Assert.True(Hub_Hm_Received_Player_Log_Text);
            Assert.True(Hub_Hm_Received_Player_Log_Minigame);

            await Delete_Should_Delete_StorySceneCommand(client, storySceneCommandModel2.Id);
            await Get_Should_Return_StorySceneCommand_NotFound(client, storySceneCommandModel2.Id);

            var updatedSceneCommandModel = await Put_Should_Update_StorySceneCommand(client, updateStoryScene, storySceneCommandModel.Id);

            await GetStoryScene_Should_Return_Ok(client, storySceneDto.Id,
                new List<CreateStorySceneCommandModel>() { updatedSceneCommandModel },
                updatedStorySceneDto);

            var readSceneModel = await StoryScenes_Get_Should_Return_StorySceneCommand(client, storySceneCommandModel.Id, updatedSceneCommandModel);
            var getReadStoryModel = await Get_Should_Return_Story(client, storyDto.Id, updatedDto, readSceneModel);
            var listStories = await Get_Should_Return_Stories(client, updatedDto, readSceneModel);

            // TODO: move this to its own test collection
            await StoryScenes_Get_Should_Return_StorySceneCommand(client, readSceneModel.Id, readSceneModel);

            await Delete_Should_Delete_Story(client, storyDto.Id);
            await Get_Should_Return_Story_NotFound(client, storyDto.Id);
            await Get_Should_Return_StorySceneCommand_NotFound(client, storyDto.Id, storySceneCommandModel.Id);

            await ImagesControllerCRUDTests.GetImageByIdAndAssertAsync(client, imageDto);
            await ImagesControllerCRUDTests.GetImageByIdAndAssertAsync(client, imageDto2);
            await AudioControllerCRUDTests.GetAudioByIdAndAssertAsync(client, audioDto);

            // TODO: check when deleting Image, Scene is not deleted
            // TODO: actually check the values (nested values)
        }

        #region Hub testing
        static async Task<HubConnection> TestGameHubAsPlayerAsync_Part1(TestServer server, string gameCode)
        {
            var gameCodeModel = new GameCodeModel(gameCode);
            var hub = CreateHubConnection(server.CreateHandler(), "game-hub");
            await hub.StartAsync();

            await hub.InvokeAsync("JoinGameAsPlayer", gameCodeModel);
            await hub.InvokeAsync("PlayerSendLog", gameCodeModel, new PlayerTextLogModel("Logged in", "Player1"));

            return hub;
        }

        static void TestGameHubAsPlayer_Part2(HubConnection hub, string gameCode)
        {
            var gameCodeModel = new GameCodeModel(gameCode);

            hub.On<ImageCommandModel>("PlayerReceiveImageCommand", async (model) =>
            {
                await hub.InvokeAsync("PlayerSendLog", gameCodeModel, new PlayerTextLogModel("Received image: " + model.ImageId, "Player1"));
            });
            hub.On<AudioCommandModel>("PlayerReceiveAudioCommand", async (model) =>
            {
                await hub.InvokeAsync("PlayerSendLog", gameCodeModel, new PlayerTextLogModel("Received audio: " + model.AudioId, "Player1"));
            });
            hub.On<TextCommandModel>("PlayerReceiveTextCommand", async (model) =>
            {
                await hub.InvokeAsync("PlayerSendLog", gameCodeModel, new PlayerTextLogModel("Received text: " + model.Text, "Player1"));
            });
            hub.On<MinigameCommandModel>("PlayerReceiveMinigameCommand", async (model) =>
            {
                await hub.InvokeAsync("PlayerSendLog", gameCodeModel, new PlayerTextLogModel("Received minigame: " + model.MinigameId, "Player1"));
            });
        }

        async Task<HubConnection> TestGameHubAsHmAsync_Part1(TestServer server, string gameCode, string token)
        {
            var hub = CreateHubConnection(server.CreateHandler(), "game-hub", token);
            await hub.StartAsync();

            await hub.InvokeAsync("JoinGameAsHm", new GameCodeModel(gameCode));

            hub.On<PlayerTextLogModel>("HmReceiveLog", (model) =>
            {
                if (!"Player1".Equals(model.PlayerName))
                    return;

                if ("Logged in".Equals(model.Message))
                {
                    Hub_Hm_Received_Player_Log_Logged_In = true;
                }
                else if ("Received image: 1".Equals(model.Message))
                {
                    Hub_Hm_Received_Player_Log_Image = true;
                }
                else if ("Received audio: 10".Equals(model.Message))
                {
                    Hub_Hm_Received_Player_Log_Audio = true;
                }
                else if ("Received text: Display this text".Equals(model.Message))
                {
                    Hub_Hm_Received_Player_Log_Text = true;
                }
                else if ("Received minigame: 9".Equals(model.Message))
                {
                    Hub_Hm_Received_Player_Log_Minigame = true;
                    semaphore.Release();
                }
            });
            return hub;
        }

        static async Task TestGameHubAsHmAsync_Part2(HubConnection hub, string gameCode)
        {
            var gameCodeModel = new GameCodeModel(gameCode);
            await hub.InvokeAsync("HmSendImageCommand", gameCodeModel, new ImageCommandModel(1));
            await hub.InvokeAsync("HmSendAudioCommand", gameCodeModel, new AudioCommandModel(10, false));
            await hub.InvokeAsync("HmSendTextCommand", gameCodeModel, new TextCommandModel("Display this text"));
            await hub.InvokeAsync("HmSendMinigameCommand", gameCodeModel, new MinigameCommandModel(9));
        }

        static async Task TestGameHubAsHm_NoToken_Exception_Async(TestServer server, string gameCode)
        {
            await using var hub = CreateHubConnection(server.CreateHandler(), "game-hub");
            await hub.StartAsync();

            var exception = await Assert.ThrowsAsync<HubException>(async () =>
            {
                await hub.InvokeAsync("JoinGameAsHm", new GameCodeModel(gameCode));
            });
            Assert.Contains("user is unauthorized", exception.Message);

            var unexpectedEx = await Assert.ThrowsAsync<HubException>(async () =>
            {
                await hub.InvokeAsync("JoinGameAsPlayer", new GameCodeModel(""));
            });
            Assert.Contains("GameCodeModel: The GameCode field is required.", unexpectedEx.Message);

            await hub.StopAsync();
        }

        static HubConnection CreateHubConnection(HttpMessageHandler handler, string hubName, string? token = default)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://localhost/{hubName}", options =>
                {
                    options.HttpMessageHandlerFactory = _ => handler;

                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .AddNewtonsoftJsonProtocol()
                .Build();

            return hubConnection;
        }
        #endregion

        #region Game testing
        static async Task<ReadGameCreatedModel> Post_Should_Create_Game(HttpClient client, long storyId)
        {
            // arrange
            var model = new CreateGameModel(storyId);

            // act
            using var response = await client.PostAsync(Path + "/game", Helper.GetContent(model));

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadGameCreatedModel>(response, StatusCodes.Status201Created);
            Assert.True(readModel.GameCode.Length > 0);
            return readModel;
        }
        static async Task<ReadGameConfiguration> Games_Get_Should_Return_GameConfiguration(HttpClient client, string gameCode)
        {
            // arrange

            // act
            using var response = await client.GetAsync(PathGames + $"/{gameCode}/configuration");

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadGameConfiguration>(response, StatusCodes.Status200OK);
            Assert.Equal(1, readModel.Audios.Count);
            Assert.Equal(1, readModel.Images.Count);
            Assert.Equal(1, readModel.Minigames.Count);
            return readModel;
        }
        #endregion

        #region File testing
        static async Task Get_Image_Should_Return_File(HttpClient client, ReadGameConfiguration config)
        {
            // arrange

            // act
            using var response = await client.GetAsync(config.Images[0].AbsoluteUrl);

            // assert
            var imageBytes = await Helper.GetFile(response);

            Assert.NotEmpty(imageBytes);
            Assert.Equal(ImagesControllerCRUDTests.JpgImageBytes, imageBytes);
        }
        static async Task Get_Audio_Should_Return_File(HttpClient client, ReadGameConfiguration config)
        {
            // arrange

            // act
            using var response = await client.GetAsync(config.Audios[0].AbsoluteUrl);

            // assert
            var audioBytes = await Helper.GetFile(response);

            Assert.NotEmpty(audioBytes);
            Assert.Equal(AudioControllerCRUDTests.AudioMp3Bytes, audioBytes);
        }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    semaphore.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
