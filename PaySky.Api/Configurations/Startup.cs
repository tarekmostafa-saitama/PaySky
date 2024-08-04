namespace PaySky.Api.Configurations;

internal static class Startup
{
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Configurations";
        var env = builder.Environment;
        builder.Configuration.AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/logging-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/logging-settings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/auth-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/auth-settings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/cors-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/cors-settings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/persistence-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/persistence-settings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/hangfire-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/hangfire-settings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/cache-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/cache-settings.{env.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();
        return builder;
    }
}