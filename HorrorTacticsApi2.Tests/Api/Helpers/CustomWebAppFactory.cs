using HorrorTacticsApi2.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests.Api.Helpers
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<HorrorDbContext>();
                services.RemoveAll<DbContextOptions<HorrorDbContext>>();

                services.AddDbContext<HorrorDbContext>(options => options.UseSqlite("Data Source=:memory:"));
            });

            return base.CreateHost(builder);
        }
    }
}
