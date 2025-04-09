namespace APPLICATION.Features.Transfers.Wallets.UpdateFunds;

public sealed record UpdateFundsRequest(string action, decimal amount);
