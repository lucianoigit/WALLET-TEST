using APPLICATION.Abstractions.Messagin;

namespace APPLICATION.Features.Transfers.Wallets.DeleteWallet;

public sealed record DeleteWalletCommand(int id):ICommand;
