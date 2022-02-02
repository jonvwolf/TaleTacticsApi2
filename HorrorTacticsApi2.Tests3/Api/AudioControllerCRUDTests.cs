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

namespace HorrorTacticsApi2.Tests3.Api
{
    public class AudioControllerCRUDTests : IClassFixture<ApiTestsCollection>
    {
        static readonly byte[] AudioMp3Bytes = Convert.FromBase64String("SUQzAwAAAAAAFFRYWFgAAAAKAAAAU29mdHdhcmUA//uQxAAAAAAAAAAAAAAAAAAAAAAAWGluZwAAAA8AAAACAAAE5ADg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg4ODg//////////////////////////////////////////////////////////////////8AAAA8TEFNRTMuMTAwBK8AAAAAAAAAABUgJAa+QQABzAAABOSpYC5WAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//vAxAAAC6wBR6AAAAntBei896SZBKSSTktaJJIAAAAABh4eHjwAAAAADDw8PfgAAH///gf/+kM/5A8PDw8M/8BAeHh4eGAAAAAAeHh4eGAAAAAAeHh4eGAAAAAAeHh4eGAAAAAAeHh4ekAAAAEB4eP/V1UxMsggAACXWXUGyGpByg5QcomompBSdCbCbCbCbCbAkCQJIUKFDEFBQoKCgoJBQUFBQoKCgoJBQUFBQoKCgoJBQUFBQoKCgoJBQUFBQoKCgoqCgoL/hQUFFQUFBf////////6DBQUFBV/+KDBQUFBQKCgoKDBQUFBQKkxBTUUzLjEwMKqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqTEFNRTMuMTAwqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqr/+xDE1gPAAAGkAAAAIAAANIAAAASqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqg==");
        readonly ApiTestsCollection _collection;
        const string Path = "secured/audios";
        public AudioControllerCRUDTests(ApiTestsCollection apiTestsCollection)
        {
            _collection = apiTestsCollection;
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            using var client = _collection.CreateClient();

            var readAudioDto = await Post_Should_Create_Audio(client, "audio1");
            await Get_Should_Return_One_Audio(client, readAudioDto);
            var readAudioDto2 = await Post_Should_Create_Audio(client, "audio2");
            var readAudioDto1_1 = await Put_Should_Update_Audio(client, readAudioDto);
            await Put_Should_Return_BadRequest(client, readAudioDto);

            await GetAudioByIdAndAssertAsync(client, readAudioDto2);
            await GetAudioByIdAndAssertAsync(client, readAudioDto1_1);

            await Get_Should_Return_Two_Audios(client, readAudioDto1_1, readAudioDto2);

            await Delete_Should_Delete_Audio(client, readAudioDto1_1);

            await Get_Should_Return_One_Audio(client, readAudioDto2);

            await Delete_Should_Delete_Audio(client, readAudioDto2);
            // TODO: check FileEntity and physical file (in memory) are also deleted
        }

        public static async Task<ReadAudioModel> Post_Should_Create_Audio(HttpClient client, string name)
        {
            // arrange
            using var form = new MultipartFormDataFile(AudioMp3Bytes, name + ".mp3");

            // act
            using var response = await client.PostAsync(Path, form.MultipartFormDataContent);

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadAudioModel>(response, StatusCodes.Status201Created);
            Assert.True(readModel.Id > 0);
            Assert.Equal(name, readModel.Name);
            Assert.Equal(FileFormatEnum.MP3, readModel.Format);
            Assert.Equal(0, (int)readModel.DurationSeconds);
            Assert.False(readModel.IsBgm);
            Assert.False(readModel.IsScanned);
            Assert.False(string.IsNullOrWhiteSpace(readModel.AbsoluteUrl));
            Assert.StartsWith("http://localhost/audios/", readModel.AbsoluteUrl);
            Assert.EndsWith(".mp3", readModel.AbsoluteUrl);

            return readModel;
        }

        static async Task Get_Should_Return_One_Audio(HttpClient client, ReadAudioModel imageDto)
        {
            // arrange
            // act
            var images = await GetAudiosAsync(client);

            // assert
            Assert.Equal(1, images?.Count);
            AssertAudioDto(imageDto, images?[0]);
        }

        static async Task<ReadAudioModel> Put_Should_Update_Audio(HttpClient client, ReadAudioModel model)
        {
            // arrange
            var updateModel = new UpdateImageModel("updated");

            // act
            using var response = await client.PutAsync(Path + $"/{model.Id}", Helper.GetContent(updateModel));

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadAudioModel>(response, StatusCodes.Status200OK);
            Assert.Equal(updateModel.Name, readModel.Name);
            Assert.Equal(model.Id, readModel.Id);
            Assert.NotEqual(model.Name, readModel.Name);
            var updated = model with { Name = updateModel.Name };
            AssertAudioDto(updated, readModel);

            return readModel;
        }

        static async Task Put_Should_Return_BadRequest(HttpClient client, ReadAudioModel model)
        {
            // arrange
            var updateModel = new UpdateImageModel("");

            // act
            using var response = await client.PutAsync(Path + $"/{model.Id}", Helper.GetContent(updateModel));
            var strResponse = await response.Content.ReadAsStringAsync();
            // assert
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Contains("The field Name must be a string or array type with a minimum length of", strResponse);
        }

        static async Task Get_Should_Return_Two_Audios(HttpClient client, ReadAudioModel imageDto, ReadAudioModel imageDto2)
        {
            // arrange
            // act
            var images = await GetAudiosAsync(client);

            // assert
            Assert.Equal(2, images?.Count);
            AssertAudioDto(imageDto, images?[0]);
            AssertAudioDto(imageDto2, images?[1]);
        }

        static async Task Delete_Should_Delete_Audio(HttpClient client, ReadAudioModel model)
        {
            // arrange

            // act
            using var response = await client.DeleteAsync(Path + $"/{model.Id}");

            // assert
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        static async Task<IList<ReadAudioModel>> GetAudiosAsync(HttpClient client)
        {
            // arrange

            // act
            using var response = await client.GetAsync(Path);

            // assert
            var images = await Helper.VerifyAndGetAsync<IList<ReadAudioModel>>(response, StatusCodes.Status200OK);

            return images;
        }

        public static async Task GetAudioByIdAndAssertAsync(HttpClient client, ReadAudioModel model)
        {
            // arrange

            // act
            using var response = await client.GetAsync(Path + "/" + model.Id);

            // assert
            var image = await Helper.VerifyAndGetAsync<ReadAudioModel>(response, StatusCodes.Status200OK);

            AssertAudioDto(model, image);
        }

        static void AssertAudioDto(ReadAudioModel expected, ReadAudioModel? imageDto)
        {
            Assert.NotNull(imageDto);
            Assert.Equal(expected.Id, imageDto?.Id);
            Assert.Equal(expected.Name, imageDto?.Name);
            Assert.Equal(expected.AbsoluteUrl, imageDto?.AbsoluteUrl);
            Assert.Equal(expected.Format, imageDto?.Format);
            Assert.Equal(expected.DurationSeconds, imageDto?.DurationSeconds);
            Assert.Equal(expected.IsBgm, imageDto?.IsBgm);
        }
    }
}
