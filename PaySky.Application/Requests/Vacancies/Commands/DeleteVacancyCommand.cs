using FluentValidation;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;
using PaySky.Shared.Exceptions;

namespace PaySky.Application.Requests.Vacancies.Commands;

public record DeleteVacancyCommand(Guid VacancyId) : ICommand<DeleteVacancyResult>;

public record DeleteVacancyResult(bool IsSuccess);

public class DeleteVacancyCommandValidator : AbstractValidator<DeleteVacancyCommand>
{
    public DeleteVacancyCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty().WithMessage("Vacancy id is required");
    }
}

internal class DeleteVacancyCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService 
        , ICurrentUserService currentUserService , IJobService jobService)
    : ICommandHandler<DeleteVacancyCommand, DeleteVacancyResult>
{
    public async Task<DeleteVacancyResult> Handle(DeleteVacancyCommand command, CancellationToken cancellationToken)
    {
        var vacancy = await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(command.VacancyId));
        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {command.VacancyId} not found");


        if (vacancy.EmployeeId != currentUserService.Id)
            throw new NotFoundException("You don't have access to this data");

        unitOfWork.VacancyRepository.Remove(vacancy);
        await unitOfWork.CommitAsync();

        jobService.Delete(vacancy.BackgroundJobId); 

        await cacheService.RemoveAsync($@"Vacancy_{vacancy.Id}", cancellationToken);

        return new DeleteVacancyResult(true);
    }
}