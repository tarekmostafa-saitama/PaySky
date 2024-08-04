using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaySky.Application.Common.Mapping;
using PaySky.Infrastructure.Authentication;
using PaySky.Infrastructure.BackgroundJobs;
using PaySky.Infrastructure.Caching;
using PaySky.Infrastructure.CommonServices;
using PaySky.Infrastructure.Cors;
using PaySky.Infrastructure.Identity;
using PaySky.Infrastructure.Middlewares;
using PaySky.Infrastructure.Persistence;
using PaySky.Infrastructure.Repositories;

namespace PaySky.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        return services
            .AddServices()
            .AddBackgroundServices(config)
            .AddCorsPolicy(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddIdentityServices()
            .AddPersistence(config)
            .AddAuthentication(config)
            .AddRepositories()
            .AddCustomMiddlewares();
    }


    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
    {
        return builder
            .UseExceptionHandlerMiddleware()
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseAuthorization()
            .UseHangfireDashboard(config);
    }
}