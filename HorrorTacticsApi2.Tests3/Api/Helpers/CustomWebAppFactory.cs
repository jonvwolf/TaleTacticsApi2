using HorrorTacticsApi2.ApiHelpers;
using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Domain.IO;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        public CustomWebAppFactoryOptions Options { get; set; } = new();
        bool _disposedValue;

        public string MainPassword { get; protected set; } = "";
        public string Token { get; protected set; } = "";

        public CustomWebAppFactory()
        {
            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(env) || !env.StartsWith(Constants.APITESTING_ENV_NAME))
                throw new InvalidOperationException($"Can't run API integration tests while not using {Constants.APITESTING_ENV_NAME} as env");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices((context, services) =>
            { 
                // Can't use anything that has log calls because Serilog will throw an exception...
                // Use ApiTestingExecutor to do operations that required logging

                services.RemoveAll<DbContextOptions<HorrorDbContext>>();
                services.RemoveAll<IHorrorDbContext>();
                services.RemoveAll<HorrorDbContext>();
                services.RemoveAll<IFileIO>();

                services.AddSingleton<IApiTestingExecutor, ApiTestingExecutor>();
                services.AddDbContext<IHorrorDbContext, HorrorDbContext>(options =>
                {
                    string connectionStrings = context.Configuration.GetConnectionString(Constants.CONNECTION_STRING_MAIN_KEY);
                    string apiTestingDbReplaceValue = context.Configuration.GetValue<string>(Constants.APITESTING_DB_REPLACE_VALUE_Key);

                    if (!connectionStrings.Contains(apiTestingDbReplaceValue))
                        throw new InvalidOperationException("Connection string does not have Db replace value: " + apiTestingDbReplaceValue);

                    // TODO: Only have 1 database for the entire API testing...
                    options.UseNpgsql(connectionStrings.Replace(apiTestingDbReplaceValue, this.Options.DbName));
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });

                services.AddSingleton<IFileIO, TestInMemoryFileIO>();

                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                MainPassword = scope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value.MainPassword;

                // TODO: this code is copied in LoginController
                var jwtGenerator = scope.ServiceProvider.GetRequiredService<IJwtGenerator>();
                Token = jwtGenerator.SerializeToken(jwtGenerator.GenerateJwtSecurityToken(new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Constants.AdminUserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sid, Constants.AdminUsername),
                    new Claim(Constants.JwtRoleKey, UserRole.Admin.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                }));

                var db = scope.ServiceProvider.GetRequiredService<HorrorDbContext>();
                db.Database.EnsureDeleted();
            });
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, Token);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!_disposedValue)
            {
                if (disposing)
                {
                    
                }

                _disposedValue = true;
            }
        }
    }
}
