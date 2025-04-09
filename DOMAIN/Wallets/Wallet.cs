using DOMAIN.Movements;
using DOMAIN.SharedKernel.Abstractions;


namespace DOMAIN.Wallets;

public sealed class Wallet : Entity<int>
{
    private readonly List<Movement> _movement = new();


    public string DocumentId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Movement> Movements => _movement.AsReadOnly();


    private Wallet(string documentId, string name, decimal initialBalance)
         : base(0)
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

    public void Transfer(Movement movement)
    {
        if (movement is null)
            throw new ArgumentNullException(nameof(movement));

        if (movement.Type != MovementType.Debit && movement.Type != MovementType.Credit)
            throw new InvalidOperationException("Invalid transfer method.");

        if (movement.Amount <= 0)
            throw new InvalidOperationException("Amount must be greater than zero.");

        if (Balance < movement.Amount)
            throw new InvalidOperationException("Insufficient funds for this transaction.");

    
        Balance -= movement.Amount;


        _movement.Add(movement);

        UpdatedAt = DateTime.UtcNow;
    }

    public void Receive(Movement movement)
    {
        if (movement.Type != MovementType.Debit && movement.Type != MovementType.Credit)
            throw new InvalidOperationException("Invalid transfer method.");

        if (movement.Amount <= 0)
            throw new InvalidOperationException("Amount must be greater than zero.");

        Balance += movement.Amount;

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateWallet(string name) // This method will be used if there are more fields that describe the wallet, it will be handled separately from adding funds.
    {
        Name = name;
    }

    public void UpdateFunds(ActionType action, decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Amount must be greater than zero.");
        }

        switch (action)
        {
            case ActionType.Add:
                Balance += amount;
                break;

            case ActionType.Withdraw:
                if (Balance < amount)
                {
                    throw new InvalidOperationException("Insufficient funds.");
                }
                Balance -= amount;
                break;

            default:
                throw new InvalidOperationException("Invalid action type.");
        }

        UpdatedAt = DateTime.UtcNow;
    }




}
