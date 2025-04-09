using APPLICATION.Abstractions.Messagin;

namespace APPLICATION.Features.Transfers.Wallets.CreateWallet;

public sealed record CreateWalletCommand(string name, string documentId, decimal balance):ICommand;
