using FluentValidation;
using Mapster;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Application.Requests.Vacancies.Models;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;
using PaySky.Shared.Exceptions;

namespace PaySky.Application.Requests.Vacancies.Commands;

public record UpdateVacancyCommand(Guid VacancyId, VacancyVm VacancyVm) : ICommand<UpdateVacancyResult>;

public record UpdateVacancyResult(VacancyVm VacancyVm);

public class UpdateVacancyCommandValidator : AbstractValidator<UpdateVacancyCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVacancyCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.VacancyVm.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.VacancyVm.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.VacancyVm.Company).NotEmpty().WithMessage("Company is required");
        RuleFor(x => x.VacancyVm.Location).NotEmpty().WithMessage("Location is required");
        RuleFor(x => x.VacancyVm.Salary).GreaterThan(0).WithMessage("Salary must be more than 0");
        RuleFor(x => x.VacancyVm.MaxApplications)
            .NotEmpty().WithMessage("Max Applications is required");
        RuleFor(x => x.VacancyVm.ExpirationDate).GreaterThan(DateTime.Today)
            .WithMessage("Expiration Date must be after the date of today");
    }
}

internal class UpdateVacancyCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService 
        , ICurrentUserService currentUserService , IJobService jobService)
    : ICommandHandler<UpdateVacancyCommand, UpdateVacancyResult>
{
    public async Task<UpdateVacancyResult> Handle(UpdateVacancyCommand command, CancellationToken cancellationToken)
    {
        var vacancy = await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(command.VacancyId));
        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {command.VacancyId} not found");

        if (vacancy.EmployeeId != currentUserService.Id)
            throw new NotFoundException("You don't have access to this data");

        vacancy.Update(
            command.VacancyVm.Title,
            command.VacancyVm.Description,
            command.VacancyVm.Company,
            command.VacancyVm.Location,
            command.VacancyVm.Salary,
            command.VacancyVm.ExpirationDate,
            command.VacancyVm.MaxApplications,
            command.VacancyVm.IsActive
        );

        unitOfWork.VacancyRepository.Update(vacancy);
        await unitOfWork.CommitAsync();


        await cacheService.RemoveAsync($@"Vacancy_{vacancy.Id}", cancellationToken);


        return new UpdateVacancyResult(vacancy.Adapt<VacancyVm>());
    }
}