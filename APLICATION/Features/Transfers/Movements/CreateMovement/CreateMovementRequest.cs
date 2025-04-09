namespace APPLICATION.Features.Transfers.Movements.CreateMovement;

public sealed record CreateMovementRequest(int walletId, int receivingWalletId, decimal ammount, string type);