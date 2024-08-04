using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;
using PaySky.Shared.Exceptions;

namespace PaySky.Application.Requests.Vacancies.Commands;

public record ToggleVacancyStatusCommand(Guid VacancyId, bool Status) : ICommand<ToggleVacancyStatusResult>;

public record ToggleVacancyStatusResult(bool IsSuccess);

internal class ToggleVacancyStatusCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService , ICurrentUserService currentUserService)
    : ICommandHandler<ToggleVacancyStatusCommand, ToggleVacancyStatusResult>
{
    public async Task<ToggleVacancyStatusResult> Handle(ToggleVacancyStatusCommand command,
        CancellationToken cancellationToken)
    {
        var vacancy = await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(command.VacancyId));
        if (vacancy == null)
            throw new NotFoundException($"Vacancy not found with id = {command.VacancyId}");

        if (vacancy.EmployeeId != currentUserService.Id)
            throw new NotFoundException("You don't have access to this data");

        vacancy.UpdateIsActiveStatus(command.Status);

        unitOfWork.VacancyRepository.Update(vacancy);
        await unitOfWork.CommitAsync();

        await cacheService.RemoveAsync($@"Vacancy_{vacancy.Id}", cancellationToken);

        return new ToggleVacancyStatusResult(true);
    }
}