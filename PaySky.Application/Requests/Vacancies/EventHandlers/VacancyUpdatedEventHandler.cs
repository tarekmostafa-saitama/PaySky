using MediatR;
using PaySky.Application.Common.Services;
using PaySky.Domain.DomainEvents;
using PaySky.Domain.Entities;

namespace PaySky.Application.Requests.Vacancies.EventHandlers;

public class VacancyUpdatedEventHandler(IJobService jobService , IVacancyService vacancyService) : INotificationHandler<VacancyUpdatedEvent>
{
    public async Task Handle(VacancyUpdatedEvent notification, CancellationToken cancellationToken)
    {

        jobService.Delete(notification.Event.BackgroundJobId);


        var delay = notification.Event.ExpirationDate - DateTime.Now; // Calculate the TimeSpan

        var jobId = jobService.Schedule(() => vacancyService.FireExpirationDateJob(notification.Event.Id.Value, false),
            delay);

        //TODO: IDK why entity isn't saved before this call already, i have a temp solution to fire it after 1 seconds
        jobService.Schedule(() => vacancyService.SetVacancyJobId(notification.Event.Id.Value, jobId), TimeSpan.FromSeconds(1));

    }
}