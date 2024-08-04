using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PaySky.Domain.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaySky.Infrastructure.Persistence.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var affectedResult = base.SavingChanges(eventData, result);
          //  DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

            return affectedResult;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var affectedResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
            Console.WriteLine("Changes have been saved to the database."); // Logging for debugging

            await DispatchDomainEvents(eventData.Context);
            Console.WriteLine("Domain events have been dispatched."); // Logging for debugging

            return affectedResult;
        }

        public async Task DispatchDomainEvents(DbContext context)
        {
            if (context == null) return;

            var entities = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            var baseEntities = entities.ToList();
            var domainEvents = baseEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            baseEntities.ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
