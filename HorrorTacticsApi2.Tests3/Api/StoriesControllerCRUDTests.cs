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
using System.Linq;

namespace HorrorTacticsApi2.Tests3.Api
{
    public class StoriesControllerCRUDTests : IClassFixture<ApiTestsCollection>, IDisposable
    {
        readonly ApiTestsCollection _collection;
        const string Path = "secured/stories";
        const string PathStoryScenes = "secured/storyscenes";
        const string PathStorySceneCommands = "secured/storyscenes/storyscenecommands";
        const string PathGames = "games";
        const string PathSecuredGames = "secured/games";

        bool Hub_Hm_Received_Hub_Command_Received;
        bool Hub_Hm_Received_Player_HmCommand_1;
        bool Hub_Hm_Received_Player_HmCommand_2;
        bool Hub_Hm_Received_Player_HmCommand_3;
        bool Hub_Hm_Received_Player_Log_Logged_In;
        bool Hub_Hm_Received_Player_HmCommandPredefined_1;
        bool Hub_Hm_Received_Player_HmCommandPredefined_2;

        private bool disposedValue;
        readonly SemaphoreSlim semaphore = new(0, 1);

        public StoriesControllerCRUDTests(ApiTestsCollection apiTestsCollection)
        {
            _collection = apiTestsCollection;
            _collection.WebAppFactory.Options.DbName = nameof(StoriesControllerCRUDTests);
        }

        public async Task<ReadStoryModel> CreateStory(HttpClient client, bool delete = false)
        {
            var imageDto = await ImagesControllerCRUDTests.Post_Should_Create_Image(client, "imagex");
            var imageDto2 = await ImagesControllerCRUDTests.Post_Should_Create_Image(client, "imagex2");

            var audioDto = await AudioControllerCRUDTests.Post_Should_Create_Audio(client, "audio");
            var audioDto2 = await AudioControllerCRUDTests.Post_Should_Create_Audio(client, "audio2");
            var audioDto3 = await AudioControllerCRUDTests.Post_Should_Create_Audio(client, "audio3");

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
                    null,
                    Minigames: new List<long>() { 1 },
                    Timers: default,
                    new List<long>() { imageDto.Id },
                    new List<long>() { audioDto.Id }
                )
            );

            var updatedStorySceneCommandDto = await EndpointHelpers.StorySceneCommandsEndpoints.PutAndAssertAsync(client, storySceneCommandDto.Id,
                new UpdateStorySceneCommandModel(
                    "Updated title ú",
                    "Text ñ Hola ñ ñ ñ",
                    Minigames: default,
                    Timers: new List<uint>() { 10 },
                    new List<long>() { imageDto.Id, imageDto2.Id },
                    new List<long>() { audioDto2.Id }
                )
            );

            var expectedStorySceneCommand1 = new ReadStorySceneCommandModel(
                updatedStorySceneCommandDto.Id,
                "Updated title ú",
                "Text ñ Hola ñ ñ ñ",
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


            //////
            var sceneToDelete = await EndpointHelpers.StorySceneEndpoints.CreateAndAssertAsync(client, storyDto.Id, new CreateStorySceneModel("scene title to delete"));
            var sceneCommandToDelete = await EndpointHelpers.StorySceneCommandsEndpoints.CreateAndAssertAsync(client, sceneToDelete.Id, new CreateStorySceneCommandModel(
                    "command 1",
                    "Text ñ Hola ñ \r\n\n\r",
                    Minigames: new List<long>() { 1 },
                    Timers: default,
                    new List<long>() { imageDto.Id },
                    new List<long>() { audioDto.Id, audioDto3.Id }
                ));

            await AudioControllerCRUDTests.Delete_Should_Delete_Audio(client, audioDto3);

            var sceneToDelete2 = sceneToDelete with
            {
                StorySceneCommands = new List<ReadStorySceneCommandModel>() { 
                    new ReadStorySceneCommandModel(
                        sceneCommandToDelete.Id,
                        "command 1",
                        "Text ñ Hola ñ ñ ñ",
                        Timers: new List<uint>(),
                        new List<ReadImageModel>() { imageDto },
                        new List<ReadAudioModel>() { audioDto },
                        new List<ReadMinigameModel>() { new ReadMinigameModel(1, "OK?") }
                    ) 
                }
            };
            var expectedStory4 = expectedStory1 with
            {
                StoryScenes = new List<ReadStorySceneModel>() { expectedStoryScene2, sceneToDelete2 }
            };
            // Test: A new scene was created with a new command. Audio was deleted and make sure the scene command wasn't deleted
            var receivedStoryDto4 = await EndpointHelpers.StoryEndpoints.GetAsync(client, storyDto.Id);
            EndpointHelpers.StoryEndpoints.AssertModels(expectedStory4, receivedStoryDto4);

            // Test: Delete command does not delete scene
            await EndpointHelpers.StorySceneCommandsEndpoints.DeleteAndAssertAsync(client, expectedStory4.StoryScenes[1].StorySceneCommands[0].Id);
            var checkSceneToDelete = await EndpointHelpers.StorySceneEndpoints.GetAsync(client, expectedStory4.StoryScenes[1].Id);
            Assert.Equal(checkSceneToDelete.Title, checkSceneToDelete.Title);

            await EndpointHelpers.StorySceneEndpoints.DeleteAndAssertAsync(client, expectedStory4.StoryScenes[1].Id);

            //////

            if (delete)
            {
                // Test: (it works) Delete scene also deletes commands
                //await EndpointHelpers.StorySceneEndpoints.DeleteAndAssertAsync(client, expectedStory4.StoryScenes[0].Id);
                //await EndpointHelpers.StorySceneCommandsEndpoints.GetAndAssertNotFoundAsync(client, expectedStory4.StoryScenes[0].StorySceneCommands[0].Id);

                await EndpointHelpers.StoryEndpoints.DeleteAndAssertAsync(client, expectedStory4.Id);

                await EndpointHelpers.StorySceneEndpoints.GetAndAssertNotFoundAsync(client, expectedStory4.StoryScenes[0].Id);
                await EndpointHelpers.StorySceneCommandsEndpoints.GetAndAssertNotFoundAsync(client, expectedStory4.StoryScenes[0].StorySceneCommands[0].Id);

                await ImagesControllerCRUDTests.GetImageByIdAndAssertAsync(client, imageDto);
                await ImagesControllerCRUDTests.GetImageByIdAndAssertAsync(client, imageDto2);
                await AudioControllerCRUDTests.GetAudioByIdAndAssertAsync(client, audioDto);
                await AudioControllerCRUDTests.GetAudioByIdAndAssertAsync(client, audioDto2);
            }
            
            return receivedStoryDto3;
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            using var client = _collection.CreateClient();
            await CreateStory(client, true);
            var storyDto = await CreateStory(client);

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

            await TestGameHubAsHmAsync_Part2(hmHub, createdGame.GameCode);

            if (!await semaphore.WaitAsync(1000))
                throw new Exception("Semaphore timed out1");

            await hub.StopAsync();
            await hmHub.StopAsync();

            Assert.True(Hub_Hm_Received_Hub_Command_Received);
            Assert.True(Hub_Hm_Received_Player_HmCommand_1);
            Assert.True(Hub_Hm_Received_Player_HmCommand_2);
            Assert.True(Hub_Hm_Received_Player_HmCommand_3);
            Assert.True(Hub_Hm_Received_Player_Log_Logged_In);
            Assert.True(Hub_Hm_Received_Player_HmCommandPredefined_1);
            Assert.True(Hub_Hm_Received_Player_HmCommandPredefined_2);

            var gameList = await GetAll_Should_Return_Games(client);
            var item = gameList.FirstOrDefault(x => x.Code == createdGame.GameCode);
            Assert.NotNull(item);
            await Delete_Should_Delete_Game(client, createdGame.GameCode);
            await Get_Should_Return_NotFound(client, createdGame.GameCode);
        }

        #region Hub testing
        static async Task<HubConnection> TestGameHubAsPlayerAsync_Part1(TestServer server, string gameCode)
        {
            var gameCodeModel = new GameCodeModel(gameCode);
            var hub = CreateHubConnection(server.CreateHandler(), "game-hub");
            await hub.StartAsync();

            await hub.InvokeAsync("JoinGameAsPlayer", gameCodeModel);
            await hub.InvokeAsync("PlayerSendLog", gameCodeModel, new TextLogModel("Logged in", "Player1"));

            return hub;
        }

        static void TestGameHubAsPlayer_Part2(HubConnection hub, string gameCode)
        {
            var gameCodeModel = new GameCodeModel(gameCode);

            hub.On("PlayerReceiveHmCommand", (Func<HmCommandModel, Task>)(async (model) =>
            {
                // player reciprocates whatever it received
                await hub.InvokeAsync("PlayerSendBackHmCommand", gameCodeModel, model);
            }));

            hub.On("PlayerReceiveHmCommandPredefined", (Func<HmCommandPredefinedModel, Task>)(async (model) =>
            {
                // player reciprocates whatever it received
                await hub.InvokeAsync("PlayerSendBackHmCommandPredefined", gameCodeModel, model);
            }));
        }

        async Task<HubConnection> TestGameHubAsHmAsync_Part1(TestServer server, string gameCode, string token)
        {
            var hub = CreateHubConnection(server.CreateHandler(), "game-hub", token);
            await hub.StartAsync();

            await hub.InvokeAsync("JoinGameAsHm", new GameCodeModel(gameCode));

            hub.On<TextLogModel>("HmReceiveLog", (model) =>
            {
                if ("Command received".Equals(model.Message))
                {
                    Hub_Hm_Received_Hub_Command_Received = true;
                }
                else if ("Logged in".Equals(model.Message) && "Player1".Equals(model.From))
                {
                    Hub_Hm_Received_Player_Log_Logged_In = true;
                }
            });

            hub.On<HmCommandModel>("HmReceiveBackHmCommand", (model) =>
            {
                if (model.ImageId == 1 && model.Text == "This is my text ñ" && model.AudioIds?.Count > 0 && model.MinigameId == 9 && model.Timer == 877)
                {
                    if (model.AudioIds[0] == 6 && model.AudioIds[1] == 7)
                    {
                        Hub_Hm_Received_Player_HmCommand_1 = true;
                    }
                }
                else if (model.ImageId == default && model.Text == "This is my text ñ" && model.AudioIds == default && model.MinigameId == default && model.Timer == default)
                {
                    Hub_Hm_Received_Player_HmCommand_2 = true;
                }
                else if (model.ImageId == 2 && model.Text == default && model.AudioIds != default && model.AudioIds[0] == 8 && model.MinigameId == default && model.Timer == default)
                {
                    Hub_Hm_Received_Player_HmCommand_3 = true;
                }
            });

            hub.On<HmCommandPredefinedModel>("HmReceiveBackHmCommandPredefined", (model) =>
            {
                if (model.ClearScreen.HasValue && model.StopBgm.HasValue && model.StopSoundEffects.HasValue)
                {
                    if (model.ClearScreen.Value && model.StopBgm.Value && model.StopSoundEffects.Value)
                    {
                        Hub_Hm_Received_Player_HmCommandPredefined_1 = true;
                    }
                }
                else if (!model.ClearScreen.HasValue && !model.StopSoundEffects.HasValue && !model.StopBgm.HasValue)
                {
                    Hub_Hm_Received_Player_HmCommandPredefined_2 = true;
                    semaphore.Release();
                }
            });

            return hub;
        }

        static async Task TestGameHubAsHmAsync_Part2(HubConnection hub, string gameCode)
        {
            var gameCodeModel = new GameCodeModel(gameCode);
            await hub.InvokeAsync("HmSendCommand", gameCodeModel, new HmCommandModel(new List<long>() { 6, 7 }, 1, 9, 877, "This is my text ñ"));
            await hub.InvokeAsync("HmSendCommand", gameCodeModel, new HmCommandModel(default, default, default, default, "This is my text ñ"));
            await hub.InvokeAsync("HmSendCommand", gameCodeModel, new HmCommandModel(new List<long>() { 8 }, 2, default, default, default));
            await hub.InvokeAsync("HmSendCommandPredefined", gameCodeModel, new HmCommandPredefinedModel(true, true, true));
            await hub.InvokeAsync("HmSendCommandPredefined", gameCodeModel, new HmCommandPredefinedModel(default, default, default));
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

        static async Task<List<ReadGameStateModel>> GetAll_Should_Return_Games(HttpClient client)
        {
            // arrange
            
            // act
            using var response = await client.GetAsync(PathSecuredGames);

            // assert
            var readModel = await Helper.VerifyAndGetAsync<List<ReadGameStateModel>>(response, StatusCodes.Status200OK);
            Assert.NotNull(readModel);
            return readModel;
        }

        static async Task Get_Should_Return_NotFound(HttpClient client, string gameCode)
        {
            // arrange

            // act
            using var response = await client.GetAsync(PathSecuredGames + "/" + gameCode);

            // assert
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        static async Task Delete_Should_Delete_Game(HttpClient client, string gameCode)
        {
            // arrange

            // act
            using var response = await client.DeleteAsync(PathSecuredGames + "/" + gameCode);

            // assert
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        static async Task<ReadGameConfiguration> Games_Get_Should_Return_GameConfiguration(HttpClient client, string gameCode)
        {
            // arrange

            // act
            using var response = await client.GetAsync(PathGames + $"/{gameCode}/configuration");

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadGameConfiguration>(response, StatusCodes.Status200OK);
            Assert.Equal(1, readModel.Audios.Count);
            Assert.Equal(2, readModel.Images.Count);
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
