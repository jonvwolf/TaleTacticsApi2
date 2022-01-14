using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Domain.IO;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        readonly SqliteInMemory _db;
        readonly CustomWebAppFactoryOptions _options;
        bool _disposedValue;

        public string MainPassword { get; protected set; } = "";
        public string Token { get; protected set; } = "";

        public CustomWebAppFactory(CustomWebAppFactoryOptions? options = null)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "ApiTesting");
            _options = options ?? new CustomWebAppFactoryOptions();
            _db = new SqliteInMemory();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<HorrorDbContext>>();
                services.AddSingleton(_db.Options);

                services.RemoveAll<IFileIO>();
                services.AddSingleton<IFileIO, TestInMemoryFileIO>();

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                MainPassword = scope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value.MainPassword;

                var jwtGenerator = scope.ServiceProvider.GetRequiredService<IJwtGenerator>();
                Token = jwtGenerator.SerializeToken(jwtGenerator.GenerateJwtSecurityToken(new List<Claim>()));
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
                    _db.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
