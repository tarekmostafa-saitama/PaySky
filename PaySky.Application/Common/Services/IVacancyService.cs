using PaySky.Domain.ValueObjects;
using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Common.Services;

public interface IVacancyService: IScopedService
{
    Task FireExpirationDateJob(Guid id, bool status);
    Task SetVacancyJobId(Guid id, string jobId);
}