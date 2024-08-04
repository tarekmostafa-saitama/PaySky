using Mapster;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Application.Requests.Vacancies.Models;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;
using PaySky.Shared.Exceptions;

namespace PaySky.Application.Requests.Vacancies.Queries;

public record GetVacancyQuery(Guid VacancyId) : IQuery<GetVacancyResult>;

public record GetVacancyResult(VacancyVm VacancyVm);

internal class GetVacancyQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService , ICurrentUserService currentUserService)
    : IQueryHandler<GetVacancyQuery, GetVacancyResult>
{
    public async Task<GetVacancyResult> Handle(GetVacancyQuery query, CancellationToken cancellationToken)
    {
        var vacancy =
            await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(query.VacancyId));
        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {query.VacancyId} not found");

        if(vacancy.EmployeeId != currentUserService.Id)
            throw new NotFoundException("You don't have access to this data");


        await cacheService.SetAsync($@"Vacancy_{vacancy.Id}", vacancy);

        return new GetVacancyResult(vacancy.Adapt<VacancyVm>());
    }
}