namespace APPLICATION.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
