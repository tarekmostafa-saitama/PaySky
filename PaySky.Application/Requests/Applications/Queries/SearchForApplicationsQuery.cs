using FluentValidation;
using Mapster;
using PaySky.Application.Common.Models;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories.Specifications;
using PaySky.Application.Repositories;
using PaySky.Application.Requests.Vacancies.Models;
using PaySky.Application.Requests.Vacancies.Queries;
using PaySky.Domain.Entities;
using PaySky.Shared.CQRS;

namespace PaySky.Application.Requests.Applications.Queries;

public record SearchForApplicationsQuery(int Page, int Size, string SearchValue) : IQuery<SearchForApplicationsResult>;

public record SearchForApplicationsResult(PaginatedResult<VacancyVm> PaginatedResult);

public class SearchForApplicationsQueryValidator : AbstractValidator<SearchForApplicationsQuery>
{
    public SearchForApplicationsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal 1.");
        RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size must be greater than 0.");
    }
}

internal class SearchForApplicationsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    : IQueryHandler<SearchForApplicationsQuery, SearchForApplicationsResult>
{
    public async Task<SearchForApplicationsResult> Handle(SearchForApplicationsQuery query, CancellationToken cancellationToken)
    {
        var specification = new Specification<Vacancy>();


        if (!string.IsNullOrWhiteSpace(query.SearchValue))
        {
            specification.ApplyCriteria(x => x.Title.Contains(query.SearchValue) ||
                                             x.Company.Contains(query.SearchValue)||
                                             x.Description.Contains(query.SearchValue)||
                                             x.Location.Contains(query.SearchValue)

                                                 );

        }

        specification.ApplyPaging(query.Size * (query.Page - 1), query.Size);
        specification.ApplyTracking(false);
        specification.ApplyOrderByDesc(x => x.CreatedAt);

        var vacancies = await unitOfWork.VacancyRepository.GetAsync(specification);
        var vacanciesCount = await unitOfWork.VacancyRepository.CountAsync(
            string.IsNullOrWhiteSpace(query.SearchValue) ? null
                : x => x.Title.Contains(query.SearchValue)
                       || x.Company.Contains(query.SearchValue)||
                       x.Description.Contains(query.SearchValue)||
                       x.Location.Contains(query.SearchValue)

        );

        return new SearchForApplicationsResult(new PaginatedResult<VacancyVm>(query.Page, query.Size, vacanciesCount,
            vacancies.Adapt<List<VacancyVm>>()));
    }
}