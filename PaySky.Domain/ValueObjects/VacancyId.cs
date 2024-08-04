using PaySky.Domain.Exceptions;

namespace PaySky.Domain.ValueObjects;

public record VacancyId
{
    private VacancyId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VacancyId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException("Vacancy Id can't be Null");

        return new VacancyId(value);
    }
}