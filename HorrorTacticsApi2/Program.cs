using HorrorTacticsApi2;
using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Domain;
using HorrorTacticsApi2.Helpers;
using Microsoft.EntityFrameworkCore;
using Serilog;

bool enableInitLogging = false;
if (args.Length > 0)
{
    foreach(var arg in args)
    {
        if ("init-log".Equals(arg, StringComparison.OrdinalIgnoreCase))
        {
            enableInitLogging = true;
        }
    }
}

var logConfig = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("./logs/ht-errors-init-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning);

if (enableInitLogging)
    logConfig.WriteTo.File("./logs/ht-log-init.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug);

Log.Logger = logConfig.CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddEnvironmentVariables(prefix: Constants.ENV_PREFIX);

    builder.Host.UseSerilog((ctx, lc) =>
    {
        lc
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration);
    });

    // Add services to the container.

    builder.Services.AddOptions<AppSettings>()
        .Bind(builder.Configuration.GetSection(Constants.APPSETTINGS_GENERAL_KEY))
        .ValidateDataAnnotations();

    builder.AddJwt();

    builder.Services.AddDbContext<HorrorDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString(Constants.CONNECTION_STRING_MAIN_KEY))
    );

    builder.Services.AddScoped<ImageModelEntityConverter>();
    builder.Services.AddScoped<ImageService>();

    builder.Services
        .AddControllers()
        .AddNewtonsoftJson();

    builder.Services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(cors =>
    {
        cors.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
    });

    {
        using var app = builder.Build();

        {
            // Start up operations...
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("HorrorTactics starting up...");
            await scope.ServiceProvider.MigrateDbAsync();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // TODO: what is the difference of having devexception page and not having it?
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSerilogRequestLogging();

        app.MapControllers();

        app.Run();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
    throw;
}
finally
{
    Log.Information("HorrorTactics shutdown.");
    Log.CloseAndFlush();
}


// This has to be added so it can be used within public classes
namespace HorrorTacticsApi2
{
    public partial class Program
    {
    }
}