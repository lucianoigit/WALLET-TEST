using APPLICATION.Abstractions.Data;
using APPLICATION.Abstractions.Messagin;
using DOMAIN.SharedKernel.Primitives;
using DOMAIN.Wallets;

namespace APPLICATION.Features.Transfers.Wallets.UpdateWallet;

public class UpdateWalletCommandHandler : ICommandHandler<UpdateWalletCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWalletRepository _walletRepository;

    public UpdateWalletCommandHandler(
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository)
    {
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
    }
    public async Task<Result> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.walletId);

        if (wallet is null) return Result.Failure(WalletErrors.WalletNotFound);

        wallet.UpdateWallet(request.name);

        _walletRepository.Update(wallet);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
