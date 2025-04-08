using DOMAIN.SharedKernel.Abstractions;
using DOMAIN.SharedKernel.Movements;


namespace DOMAIN.SharedKernel.Wallet;

public sealed class Wallet : Entity<int>
{
    private readonly List<Movement> _movement = new();


    public string DocumentId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Movement> Transactions => _movement.AsReadOnly();


    private Wallet(string documentId, string name, decimal initialBalance)
    {
        DocumentId = documentId;
        Name = name;
        Balance = initialBalance;
        CreatedAt = DateTime.UtcNow;
    }

    private Wallet() { }

    public static Wallet Create(string documentId, string name, decimal initialBalance)
    {
        return new Wallet(documentId, name, initialBalance);
    }
}
