using APPLICATION.Abstractions.Messagin;
using APPLICATION.Abstractions.Pagination;
using APPLICATION.Helpers;
using DOMAIN.Movements;

namespace APPLICATION.Features.Transfers.Movements.GetAllMovement;

public sealed class GetAllMovementQueryHandler : IQueryHandler<GetAllMovementQuery, IPagedList<MovementResponse>>
{
    private readonly IMovementRepository _movementRepository;
    private readonly IPaginationService _paginationService;

    public GetAllMovementQueryHandler(
        IMovementRepository movementRepository,
        IPaginationService paginationService)
    {
        _movementRepository = movementRepository;
        _paginationService = paginationService;
    }

    public async Task<IPagedList<MovementResponse>> Handle(GetAllMovementQuery request, CancellationToken cancellationToken)
    {
        var movements = await _movementRepository.GetAllAsync(cancellationToken);

        if (movements is null || movements.Count == 0)
        {
            return _paginationService.CreatePaginateResponse(
                new List<MovementResponse>(), request.take, request.offset, 0);
        }

        var responseList = movements
            .Select(m => new MovementResponse(
                m.Id,
                m.WalletId,
                m.Amount,
                m.Type,
                m.ReceivingWalletId,
                m.Date))
            .ToList();

        var paginated = _paginationService.PaginateList(
            responseList,
            request.take,
            request.offset,
            responseList.Count);

        return paginated;
    }
}
