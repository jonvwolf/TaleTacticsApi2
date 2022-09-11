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


            var created = await CreateUser(client);
            var created2 = await CreateUser(client);
            var getUser = await GetUser(client, created.Id);
            var updateUser = await UpdateUser(client, created.Id, new UpdateUserModel("mynewpasswordtest"));

            Assert.Equal(created.Username, getUser.Username);
            Assert.Equal(created.Role, getUser.Role);
        }

        static async Task<ReadUserModel> CreateUser(HttpClient client)
        {
            using var response = await client.PostAsync(Path, Helper.GetContent(new CreateUserModel("test1", "passwordtest")));

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
