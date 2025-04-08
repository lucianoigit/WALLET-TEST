namespace DOMAIN.SharedKernel.Movements;

public sealed class Movement
{
    public int Id { get; private set; }
    public decimal Amount { get; private set; }
    public MovementType Type { get; private set; }
    public DateTime Date { get; private set; }

    private Movement() { }

    public Movement(decimal amount, MovementType type)
    {
        Amount = amount;
        Type = type;
        Date = DateTime.UtcNow;
    }
}
