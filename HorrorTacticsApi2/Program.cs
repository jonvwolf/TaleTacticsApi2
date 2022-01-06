using HorrorTacticsApi2;
using HorrorTacticsApi2.Data;
using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("./logs/ht-logs-.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 5242880, retainedFileCountLimit: 10)
    .WriteTo.File("./logs/ht-errors-init-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .CreateBootstrapLogger();

Log.Information("HorrorTactics starting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) =>
    {
        lc.WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration);
    });

    builder.Configuration.AddEnvironmentVariables(prefix: Constants.ENV_PREFIX);
    // Add services to the container.

    builder.Services.AddOptions<AppSettings>()
        .Bind(builder.Configuration.GetSection(Constants.APPSETTINGS_GENERAL_KEY))
        .ValidateDataAnnotations();
    {
        // Jwt setup
        builder.Services.AddOptions<JwtGeneratorStaticOptions>()
            .Bind(builder.Configuration.GetSection(Constants.APPSETTINGS_JWTGENERATOR_KEY))
            .ValidateDataAnnotations();

        builder.Services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
    }

    builder.Services.AddDbContext<HorrorDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString(Constants.CONNECTION_STRING_MAIN_KEY))
    );

    builder.Services
        .AddControllers()
        .AddNewtonsoftJson();

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

    var app = builder.Build();

    {
        // TODO: make sure this is not called when replacing services in API integration
        using var scope2 = app.Services.CreateScope();
        // Making sure the database file is always updated
        var db = scope2.ServiceProvider.GetRequiredService<HorrorDbContext>();
        await db.Database.MigrateAsync();
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

    app.UseSerilogRequestLogging();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("HorrorTactics shutdown.");
    Log.CloseAndFlush();
}


// This has to be added so it can be used within public classes
namespace HorrorTacticsApi2
{
    public partial class Program { }
}