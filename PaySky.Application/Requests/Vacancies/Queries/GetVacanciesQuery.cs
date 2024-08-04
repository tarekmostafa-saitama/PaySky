using FluentValidation;
using Mapster;
using PaySky.Application.Common.Models;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Application.Repositories.Specifications;
using PaySky.Application.Requests.Vacancies.Models;
using PaySky.Domain.Entities;
using PaySky.Shared.CQRS;

namespace PaySky.Application.Requests.Vacancies.Queries;

public record GetVacanciesQuery(int Page, int Size, string SearchValue) : IQuery<GetVacanciesResult>;

public record GetVacanciesResult(PaginatedResult<VacancyVm> PaginatedResult);

public class GetVacanciesQueryValidator : AbstractValidator<GetVacanciesQuery>
{
    public GetVacanciesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal 1.");
        RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size must be greater than 0.");
    }
}

internal class GetVacanciesQueryHandler(IUnitOfWork unitOfWork , ICurrentUserService  currentUserService)
    : IQueryHandler<GetVacanciesQuery, GetVacanciesResult>
{
    public async Task<GetVacanciesResult> Handle(GetVacanciesQuery query, CancellationToken cancellationToken)
    {
        var specification = new Specification<Vacancy>();

        specification.ApplyCriteria(x => x.EmployeeId == currentUserService.Id);

        if (!string.IsNullOrWhiteSpace(query.SearchValue))
            specification.ApplyCriteria(x => x.Title.Contains(query.SearchValue));

        specification.ApplyPaging(query.Size * (query.Page - 1), query.Size);
        specification.ApplyTracking(false);
        specification.ApplyOrderByDesc(x => x.CreatedAt);

        var vacancies = await unitOfWork.VacancyRepository.GetAsync(specification);
        var vacanciesCount = await unitOfWork.VacancyRepository.CountAsync(
            string.IsNullOrWhiteSpace(query.SearchValue) ? x => x.EmployeeId == currentUserService.Id
                : x => x.Title.Contains(query.SearchValue) &&
                       x.EmployeeId == currentUserService.Id
        );

        return new GetVacanciesResult(new PaginatedResult<VacancyVm>(query.Page, query.Size, vacanciesCount,
            vacancies.Adapt<List<VacancyVm>>()));
    }
}