namespace APPLICATION.Features.Transfers.Wallets.GetAllWallet;

public sealed record WalletResponse(
    int id,
    string documentId,
    string name,
    decimal balance,
    DateTime createdAt,
    DateTime? updatedAt
);
