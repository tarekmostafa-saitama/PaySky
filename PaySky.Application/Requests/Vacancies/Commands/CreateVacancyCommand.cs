using FluentValidation;
using Mapster;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Application.Requests.Vacancies.Models;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;

namespace PaySky.Application.Requests.Vacancies.Commands;

public record CreateVacancyCommand(VacancyVm VacancyVm) : ICommand<CreateVacancyResult>;

public record CreateVacancyResult(VacancyVm VacancyVm);

public class CreateVacancyCommandValidator : AbstractValidator<CreateVacancyCommand>
{
    public CreateVacancyCommandValidator()
    {
        RuleFor(x => x.VacancyVm.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.VacancyVm.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.VacancyVm.Company).NotEmpty().WithMessage("Company is required");
        RuleFor(x => x.VacancyVm.Location).NotEmpty().WithMessage("Location is required");
        RuleFor(x => x.VacancyVm.Salary).GreaterThan(0).WithMessage("Salary must be more than 0");
        RuleFor(x => x.VacancyVm.MaxApplications)
            .GreaterThan(0).WithMessage("Max Applications must be greater than 0");
        RuleFor(x => x.VacancyVm.ExpirationDate).GreaterThan(DateTime.Today)
            .WithMessage("Expiration Date must be after the date of today");
    }
}

internal class CreateVacancyCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService ,ICurrentUserService currentUserService)
    : ICommandHandler<CreateVacancyCommand, CreateVacancyResult>
{
    public async Task<CreateVacancyResult> Handle(CreateVacancyCommand command, CancellationToken cancellationToken)
    {
        var vacancy = Vacancy.Create(VacancyId.Of(Guid.NewGuid()),
            command.VacancyVm.Title,
            command.VacancyVm.Description,
            command.VacancyVm.Company,
            command.VacancyVm.Location,
            command.VacancyVm.Salary,
            command.VacancyVm.ExpirationDate,
            command.VacancyVm.MaxApplications,
            null,
            currentUserService.Id,
            true
        );

        unitOfWork.VacancyRepository.Add(vacancy);
        await unitOfWork.CommitAsync();


        return new CreateVacancyResult(vacancy.Adapt<VacancyVm>());
    }
}