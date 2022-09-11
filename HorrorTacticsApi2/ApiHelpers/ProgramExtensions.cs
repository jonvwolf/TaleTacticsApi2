using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Migrations;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace HorrorTacticsApi2.Helpers
{
    public static class ProgramExtensions
    {
        public static WebApplicationBuilder AddJwt(this WebApplicationBuilder builder)
        {
            // Jwt setup
            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

            builder.Services.AddOptions<JwtGeneratorStaticOptions>()
                .Bind(builder.Configuration.GetSection(Constants.APPSETTINGS_JWTGENERATOR_KEY))
                .ValidateDataAnnotations();

            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // This is to get access_token for hub connections
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(path))
                            return Task.CompletedTask;

                        if (path.StartsWithSegments(Constants.HUB_PATH))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            return builder;
        }

        public static async Task MigrateDbAsync(this IServiceProvider services)
        {
            // Making sure the database file is always updated
            var db = services.GetRequiredService<HorrorDbContext>();
            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                var settings = services.GetRequiredService<IOptions<AppSettings>>();

                bool applyMigrations = false;
                if (settings.Value.ByPassApplyMigrationFileCheck)
                    applyMigrations = true;

                // If no migrations have been applied, it means the database is empty
                // TODO: change this to `nameof(Initialization)`
                if (!string.IsNullOrEmpty(pendingMigrations.FirstOrDefault(x => x.EndsWith($"_{nameof(Initialization)}"))))
                    applyMigrations = true;

                if (applyMigrations)
                {
                    logger.LogInformation("Applying migrations...");
                    await db.Database.MigrateAsync();
                    logger.LogInformation("Finished migrations.");

                    var appsettings = services.GetRequiredService<IOptions<AppSettings>>();
                    var passwordHelper = services.GetRequiredService<PasswordHelper>();

                    var salt = passwordHelper.GenerateSalt();
                    var password = passwordHelper.GenerateHash(appsettings.Value.MainPassword, salt);
                    var context = services.GetRequiredService<HorrorDbContext>();

                    //TODO: this should be in domain
                    var user = await context.Users.SingleAsync(x => x.Id == Constants.AdminUserId);
                    user.Password = password;
                    user.Salt = salt;

                    await context.SaveChangesWrappedAsync();

                    return;
                }

                if (File.Exists(Constants.FILE_APPLY_MIGRATIONS))
                {
                    logger.LogInformation("Applying migrations...");
                    await db.Database.MigrateAsync();
                    logger.LogInformation("Finished migrations.");

                    File.Delete(Constants.FILE_APPLY_MIGRATIONS);
                    logger.LogInformation($"File {Constants.FILE_APPLY_MIGRATIONS} deleted.");
                }
                else
                {
                    throw new InvalidOperationException($"There are pending migrations. " +
                    $"Backup the database file then create '{Constants.FILE_APPLY_MIGRATIONS}' file and run HorrorTactics again");
                }
            }
        }
    }
}
