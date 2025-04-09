using APPLICATION.Abstractions.Messagin;

namespace APPLICATION.Features.Transfers.Movements.CreateMovement;

public sealed record CreateMovementCommand(int walletId, int receivingWalletId,decimal ammount, string type):ICommand;
