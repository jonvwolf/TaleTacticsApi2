using HorrorTacticsApi2.ApiHelpers;
using HorrorTacticsApi2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    public class ApiTestingExecutor : IApiTestingExecutor
    {
        public async Task StartAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HorrorDbContext>();
            await db.Database.EnsureDeletedAsync();
        }
    }
}
