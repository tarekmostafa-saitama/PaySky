using PaySky.Application.Common.Caching;
using PaySky.Application.Repositories;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;
using PaySky.Shared.Exceptions;

namespace PaySky.Application.Requests.Vacancies.Commands;

public record SetVacancyJobIdCommand(Guid VacancyId, string JobId) : ICommand<bool>;

internal class SetVacancyJobIdCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    : ICommandHandler<SetVacancyJobIdCommand, bool>
{
    public async Task<bool> Handle(SetVacancyJobIdCommand command, CancellationToken cancellationToken)
    {
        var vacancy = await unitOfWork.VacancyRepository.GetSingleAsync(x => x.Id == VacancyId.Of(command.VacancyId));
        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {command.VacancyId} not found");
        vacancy.SetJobId(command.JobId);

        unitOfWork.VacancyRepository.Update(vacancy);
        await unitOfWork.CommitAsync();

        await cacheService.RemoveAsync($@"Vacancy_{vacancy.Id}", cancellationToken);

        return true;
    }
}