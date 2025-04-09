using APPLICATION.Abstractions.Data;
using APPLICATION.Abstractions.Messagin;
using DOMAIN.SharedKernel.Primitives;
using DOMAIN.Wallets;

namespace APPLICATION.Features.Transfers.Wallets.CreateWallet;

public class CreateWalletCommandHandler : ICommandHandler<CreateWalletCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWalletRepository _walletRepository;

    public CreateWalletCommandHandler(
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository)
    {
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await _walletRepository.ExistsAsync(request.documentId, cancellationToken);

        if (alreadyExists) return Result.Failure(WalletErrors.WalletAlreadyExists);

        var wallet = Wallet.Create(
            request.documentId,
            request.name,
            request.balance);

        _walletRepository.Add(wallet);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
