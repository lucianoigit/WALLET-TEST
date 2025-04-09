using APPLICATION.Abstractions.Messagin;
using APPLICATION.Abstractions.Pagination;
using APPLICATION.Helpers;
using DOMAIN.Wallets;

namespace APPLICATION.Features.Transfers.Wallets.GetAllWallet;

public class GetAllWalletQueryHandler : IQueryHandler<GetAllWalletQuery, IPagedList<WalletResponse>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPaginationService _paginationService;

    public GetAllWalletQueryHandler(
        IWalletRepository walletRepository,
        IPaginationService paginationService)
    {
        _walletRepository = walletRepository;
        _paginationService = paginationService;
    }

    public async Task<IPagedList<WalletResponse>> Handle(GetAllWalletQuery request, CancellationToken cancellationToken)
    {
        var wallets = await _walletRepository.GetAllAsync(cancellationToken);

        if (wallets is null || wallets.Count == 0)
        {
            return _paginationService.CreatePaginateResponse(
                new List<WalletResponse>(), request.take, request.offset, 0);
        }

        var responseList = wallets
            .Select(w => new WalletResponse(
                w.Id,
                w.DocumentId,
                w.Name,
                w.Balance,
                w.CreatedAt,
                w.UpdatedAt))
            .ToList();

        var paginated =  _paginationService.PaginateList(
            responseList,
            request.take,
            request.offset,
            responseList.Count);

        return paginated;
    }
}
