using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Tests3.Api.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        readonly SqliteInMemory _db;
        readonly CustomWebAppFactoryOptions _options;
        bool _disposedValue;
        public CustomWebAppFactory(CustomWebAppFactoryOptions? options = null)
        {
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
            });
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
