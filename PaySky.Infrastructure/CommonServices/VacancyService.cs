using MediatR;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Requests.Vacancies.Commands;
using PaySky.Domain.ValueObjects;
using PaySky.Infrastructure.Repositories;
using PaySky.Shared.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;
using Mapster;
using PaySky.Application.Repositories;
using PaySky.Domain.Entities;

namespace PaySky.Infrastructure.CommonServices;

public class VacancyService(ISender sender , IUnitOfWork unitOfWork , ICacheService cacheService) : IVacancyService
{
    public async Task FireExpirationDateJob(Guid id, bool status)
    {

        var vacancy = await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(id));
        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {id} not found");

        unitOfWork.VacancyRepository.Remove(vacancy);

        unitOfWork.ArchivedVacancyRepository.Add(vacancy.Adapt<ArchivedVacancy>());

        await unitOfWork.CommitAsync();

    }

    public async Task SetVacancyJobId(Guid id, string jobId)
    {
        var vacancy = await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(id));
        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {id} not found");
        vacancy.SetJobId(jobId);

        unitOfWork.VacancyRepository.Update(vacancy);
        await unitOfWork.CommitAsync();

        await cacheService.RemoveAsync($@"Vacancy_{vacancy.Id}");

    }
}