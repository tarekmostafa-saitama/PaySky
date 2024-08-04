using PaySky.Domain.Exceptions;

namespace PaySky.Domain.ValueObjects;

public record ApplicationId
{
    private ApplicationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ApplicationId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException("Application Id can't be Null");

        return new ApplicationId(value);
    }
}