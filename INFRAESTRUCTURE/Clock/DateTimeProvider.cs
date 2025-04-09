using APPLICATION.Abstractions.Clock;

namespace INFRAESTRUCTURE.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
