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

namespace HorrorTacticsApi2.Tests3.Api
{
    public class ImagesControllerCRUDTests2 : IClassFixture<ApiTestsCollection>
    {
        readonly ApiTestsCollection _collection;
        const string Path = "secured/images";
        public ImagesControllerCRUDTests2(ApiTestsCollection apiTestsCollection)
        {
            _collection = apiTestsCollection;
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            var client = _collection.CreateClient();

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
            var client = _collection.CreateClient();

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
            var createImageDto = new CreateImageModel(name, new byte[1] { 2 });

            // act
            var response = await client.PostAsync(Path, Helper.GetContent(createImageDto));

            // assert
            var readModel = await Helper.VerifyAndGetAsync<ReadImageModel>(response, StatusCodes.Status201Created);
            Assert.Equal(createImageDto.Name, readModel.Name);
            // TODO: validate other properties

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
            var updateModel = new UpdateImageModel("updated", new byte[] { 1 });

            // act
            var response = await client.PutAsync(Path + $"/{model.Id}", Helper.GetContent(updateModel));

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
            var response = await client.DeleteAsync(Path + $"/{model.Id}");

            // assert
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        static async Task<IList<ReadImageModel>> GetImagesAndValidateTaskAsync(HttpClient client)
        {
            // arrange

            // act
            var response = await client.GetAsync(Path);

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
