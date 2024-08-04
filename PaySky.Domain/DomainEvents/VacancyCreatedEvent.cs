using PaySky.Domain.Common;
using PaySky.Domain.Entities;

namespace PaySky.Domain.DomainEvents;

public record VacancyCreatedEvent(Vacancy Event) : IDomainEvent;