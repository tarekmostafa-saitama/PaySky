
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using PaySky.Api.Configurations;
using PaySky.Application;
using PaySky.Infrastructure;
using PaySky.Infrastructure.Logging;
using PaySky.Infrastructure.Logging.SerilogSettings;
using PaySky.Infrastructure.Persistence;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddEndpointsApiExplorer();

   


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        
    }
    else
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    var initialiser =
        scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.SeedAsync();



    app.UseInfrastructure(builder.Configuration);
   
    app.MapControllers();
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

public partial class Program
{
}