using HorrorTacticsApi2.Data;
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

namespace HorrorTacticsApi2.Tests2.Api.Helpers
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        readonly SqliteInMemory _db;
        readonly CustomWebAppFactoryOptions _options;
        public CustomWebAppFactory(CustomWebAppFactoryOptions? options = null)
        {
            _options = options ?? new CustomWebAppFactoryOptions();
            _db = new SqliteInMemory();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseSerilog((ctx, lc) =>
            {
                lc
                    .WriteTo.Console();
            });

            builder.ConfigureServices(services =>
            {
                //services.RemoveAll<HorrorDbContext>();
                services.RemoveAll<DbContextOptions<HorrorDbContext>>();
                //services.AddDbContext<HorrorDbContext>(options => options.UseSqlite($"Data Source={_options.DbFileName}"));

                services.AddSingleton(_db.Options);
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}
