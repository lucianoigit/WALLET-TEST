namespace APPLICATION.Features.Transfers.Wallets.CreateWallet;

public sealed record CreateWalletRequest(string name, string documentId, decimal balance);
