using DOMAIN.Movements;

namespace APPLICATION.Features.Transfers.Movements.GetAllMovement;

public sealed record MovementResponse(
    int id,
    int walletId,
    decimal amount,
    MovementType yype,
    int? receivingWalletId,
    DateTime date);
