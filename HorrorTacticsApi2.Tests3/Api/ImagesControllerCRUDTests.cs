using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Domain.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Logging;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Xunit;
using System.Net.Http.Headers;
using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Tests3.Api
{
    public class ImagesControllerCRUDTests : IClassFixture<ApiTestsCollection>
    {
        const string JpgImageBase64 = "/9j/4AAQSkZJRgABAQEAYABgAAD/4QA6RXhpZgAATU0AKgAAAAgAA1EAAAQAAAABAAAAAFEBAAMAAAABAAEAAFEEAAEAAAAB/AAAAAAAAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAACAAIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD9vZ55PPf52+8e9FFFfwPiP40vV/mfTx2P/9k=";
        readonly ApiTestsCollection _collection;
        const string Path = "secured/images";
        public ImagesControllerCRUDTests(ApiTestsCollection apiTestsCollection)
        {
            _collection = apiTestsCollection;
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            using var client = _collection.CreateClient();

            var readImageDto = await Post_Should_Create_Image(client, "image1");
            await Get_Should_Return_One_Image(client, readImageDto);
            var readImageDto2 = await Post_Should_Create_Image(client, "image2");
            var readImageDto1_1 = await Put_Should_Update_Image(client, readImageDto);

            await Get_Should_Return_Two_Images(client, readImageDto1_1, readImageDto2);

            await Delete_Should_Delete_Image(client, readImageDto1_1);

            await Get_Should_Return_One_Image(client, readImageDto2);

            await Delete_Should_Delete_Image(client, readImageDto2);
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors2()
        {
            using var client = _collection.CreateClient();

            var readImageDto = await Post_Should_Create_Image(client, "image1");
            await Get_Should_Return_One_Image(client, readImageDto);
            var readImageDto2 = await Post_Should_Create_Image(client, "image2");
            var readImageDto1_1 = await Put_Should_Update_Image(client, readImageDto);

            await Get_Should_Return_Two_Images(client, readImageDto1_1, readImageDto2);

            await Delete_Should_Delete_Image(client, readImageDto1_1);

            await Get_Should_Return_One_Image(client, readImageDto2);

            await Delete_Should_Delete_Image(client, readImageDto2);
        }

        static async Task<ReadImageModel> Post_Should_Create_Image(HttpClient client, string name)
        {
            // arrange
            using var form = new MultipartFormDataContent();
            var imageBytes = Convert.FromBase64String(JpgImageBase64);
            using var memoryStream = new MemoryStream(imageBytes);
            using var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = name + ".jpg"
            };

            form.Add(streamContent, "testx", "testx.jpg");

            // act
            using var response = await client.PostAsync(Path, form);

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadImageModel>(response, StatusCodes.Status201Created);
            Assert.True(readModel.Id > 0);
            Assert.Equal(name + ".jpg", readModel.Name);
            Assert.Equal(FileFormatEnum.JPG, readModel.Format);
            Assert.Equal(0, (int)readModel.Height);
            Assert.Equal(0, (int)readModel.Width);
            Assert.False(readModel.IsScanned);
            Assert.Equal(name + ".jpg", readModel.AbsoluteUrl);

            return readModel;
        }

        static async Task Get_Should_Return_One_Image(HttpClient client, ReadImageModel imageDto)
        {
            // arrange
            // act
            var images = await GetImagesAndValidateTaskAsync(client);

            // assert
            Assert.Equal(1, images?.Count);
            AssertImageDto(imageDto, images?[0]);
        }

        static async Task<ReadImageModel> Put_Should_Update_Image(HttpClient client, ReadImageModel model)
        {
            // arrange
            var updateModel = new UpdateImageModel("updated");

            // act
            using var response = await client.PutAsync(Path + $"/{model.Id}", Helper.GetContent(updateModel));

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadImageModel>(response, StatusCodes.Status200OK);
            Assert.Equal(updateModel.Name, readModel.Name);
            Assert.Equal(model.Id, readModel.Id);
            Assert.NotEqual(model.Name, readModel.Name);
            // TODO: validate other properties

            return readModel;
        }

        static async Task Get_Should_Return_Two_Images(HttpClient client, ReadImageModel imageDto, ReadImageModel imageDto2)
        {
            // arrange
            // act
            var images = await GetImagesAndValidateTaskAsync(client);

            // assert
            Assert.Equal(2, images?.Count);
            AssertImageDto(imageDto, images?[0]);
            AssertImageDto(imageDto2, images?[1]);
        }

        static async Task Delete_Should_Delete_Image(HttpClient client, ReadImageModel model)
        {
            // arrange
            
            // act
            using var response = await client.DeleteAsync(Path + $"/{model.Id}");

            // assert
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        static async Task<IList<ReadImageModel>> GetImagesAndValidateTaskAsync(HttpClient client)
        {
            // arrange

            // act
            using var response = await client.GetAsync(Path);

            // assert
            var images = await Helper.VerifyAndGetAsync<IList<ReadImageModel>>(response, StatusCodes.Status200OK);

            return images;
        }

        static void AssertImageDto(ReadImageModel expected, ReadImageModel? imageDto)
        {
            Assert.Equal(expected.Id, imageDto?.Id);
            Assert.Equal(expected.Name, imageDto?.Name);
            Assert.Equal(expected.AbsoluteUrl, imageDto?.AbsoluteUrl);
            Assert.Equal(expected.Format, imageDto?.Format);
            Assert.Equal(expected.Height, imageDto?.Height);
            Assert.Equal(expected.Width, imageDto?.Width);
        }
    }
}
