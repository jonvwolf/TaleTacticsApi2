using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Tests.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HorrorTacticsApi2.Tests.Api
{
    public class ImagesControllerCRUDTests : IClassFixture<CustomWebAppFactory>
    {
        readonly CustomWebAppFactory _factory;
        const string Path = "api/images";
        public ImagesControllerCRUDTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        async Task<IList<ReadImageModel>> GetImagesTaskAsync(HttpClient client)
        {
            // arrange

            // act
            var response = await client.GetAsync(Path);
            var respStr = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var images = JsonConvert.DeserializeObject<IList<ReadImageModel>>(respStr);
            Assert.NotNull(images);

            if (images == null)
                throw new InvalidOperationException("This will never be executed");

            return images;
        }

        [Fact]
        public async Task Should_Do_Crud_Without_Errors()
        {
            var client = _factory.CreateClient();

            var readImageDto = await Post_Should_Create_Image(client);
            await Get_Should_Return_One_Image(client, readImageDto);
            var readImageDto2 = await Post_Should_Create_Image(client);

            await Get_Should_Return_Two_Images(client, readImageDto, readImageDto2);
        }

        async Task<ReadImageModel> Post_Should_Create_Image(HttpClient client)
        {
            // arrange
            var createImageDto = new CreateImageModel();

            // act

            // assert

            return new ReadImageModel();
        }

        async Task Get_Should_Return_One_Image(HttpClient client, ReadImageModel imageDto)
        {
            // arrange
            // act
            var images = await GetImagesTaskAsync(client);

            // assert
            Assert.Equal(1, images?.Count);
            AssertImageDto(imageDto, images?[0]);
        }

        async Task Get_Should_Return_Two_Images(HttpClient client, ReadImageModel imageDto, ReadImageModel imageDto2)
        {
            // arrange
            // act
            var images = await GetImagesTaskAsync(client);

            // assert
            Assert.Equal(2, images?.Count);
            AssertImageDto(imageDto, images?[0]);
            AssertImageDto(imageDto2, images?[1]);
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
