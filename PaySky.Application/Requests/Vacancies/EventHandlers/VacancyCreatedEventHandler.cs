using MediatR;
using PaySky.Application.Common.Services;
using PaySky.Application.Requests.Vacancies.Commands;
using PaySky.Domain.DomainEvents;

namespace PaySky.Application.Requests.Vacancies.EventHandlers;

public class VacancyCreatedEventHandler(IVacancyService vacancyService, IJobService jobService, ISender sender)
    : INotificationHandler<VacancyCreatedEvent>
{
    public async Task Handle(VacancyCreatedEvent notification, CancellationToken cancellationToken)
    {
        var delay = notification.Event.ExpirationDate - DateTime.Now; // Calculate the TimeSpan

        var jobId = jobService.Schedule(() => vacancyService.FireExpirationDateJob(notification.Event.Id.Value, false),
            delay);

        //TODO: IDK why entity isn't saved before this call already, i have a temp solution to fire it after 1 seconds
        jobService.Schedule(() => vacancyService.SetVacancyJobId(notification.Event.Id.Value, jobId),TimeSpan.FromSeconds(1));
    }
}