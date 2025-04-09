using APPLICATION.Abstractions.Data;
using APPLICATION.Abstractions.Messagin;
using DOMAIN.SharedKernel.Primitives;
using DOMAIN.Wallets;

namespace APPLICATION.Features.Transfers.Wallets.DeleteWallet;

public class DeleteWalletCommandHandler : ICommandHandler<DeleteWalletCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWalletRepository _walletRepository;

    public DeleteWalletCommandHandler(
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository
        )
    { 
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
    }
    public async Task<Result> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.id);

        if (wallet is null) return Result.Failure(WalletErrors.WalletNotFound);

        _walletRepository.Remove(wallet);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
