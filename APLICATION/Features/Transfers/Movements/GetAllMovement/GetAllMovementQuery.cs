using APPLICATION.Abstractions.Messagin;
using APPLICATION.Helpers;

namespace APPLICATION.Features.Transfers.Movements.GetAllMovement;

public sealed record GetAllMovementQuery(int take, int offset):IQuery<IPagedList<MovementResponse>>;
