using APPLICATION.Abstractions.Messagin;

namespace APPLICATION.Features.Transfers.Wallets.UpdateWallet;

public sealed record UpdateWalletCommand(int walletId,string name):ICommand;
