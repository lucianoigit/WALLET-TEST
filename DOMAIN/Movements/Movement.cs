using DOMAIN.SharedKernel.Abstractions;

namespace DOMAIN.Movements;

public sealed class Movement : Entity<int>
{
    public int WalletId { get; private set; }
    public decimal Amount { get; private set; }
    public MovementType Type { get; private set; }
    public int? ReceivingWalletId { get; private set; }
    public DateTime Date { get; private set; }

    private Movement() { }

    private Movement(int walletId,int receivingWalletId, decimal amount, MovementType type)
        : base(0) // El ID lo genera EF, usamos 0 como valor por defecto.
    {
        WalletId = walletId;
        ReceivingWalletId = receivingWalletId;
        Amount = amount;
        Type = type;
        Date = DateTime.UtcNow;
    }

    public static Movement Create(int walletId,int receivingWalletId, decimal amount, MovementType type)
    {
        return new Movement(walletId, receivingWalletId, amount, type);
    }
}
