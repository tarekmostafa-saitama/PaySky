using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Common.Services;

public interface ICurrentUserService : IScopedService
{
    string Id { get; }
}