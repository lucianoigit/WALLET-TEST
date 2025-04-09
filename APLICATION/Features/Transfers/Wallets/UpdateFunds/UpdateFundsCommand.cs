using APPLICATION.Abstractions.Messagin;

namespace APPLICATION.Features.Transfers.Wallets.UpdateFunds;

public sealed record UpdateFundsCommand(int id, string action, decimal amount):ICommand;
