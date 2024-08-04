using Microsoft.Extensions.DependencyInjection;
using PaySky.Application.Repositories;

namespace PaySky.Infrastructure.Repositories;

internal static class Startup
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        return services;
    }
}