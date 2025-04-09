using APPLICATION.Abstractions.Data;
using APPLICATION.Abstractions.Messagin;
using DOMAIN.Movements;
using DOMAIN.SharedKernel.Primitives;
using DOMAIN.Wallets;

namespace APPLICATION.Features.Transfers.Wallets.UpdateFunds;

public sealed class UpdateFundsCommandHandler : ICommandHandler<UpdateFundsCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWalletRepository _walletRepository;

    public UpdateFundsCommandHandler(
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository)
    { 
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
    }
    public async Task<Result> Handle(UpdateFundsCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ActionType>(request.action, ignoreCase: true, out var actionType))
        {
            return Result.Failure(WalletErrors.InvalidActionType);
        }

        var wallet = await _walletRepository.GetByIdAsync(request.id);

        if (wallet is null) return Result.Failure(WalletErrors.WalletNotFound);

        try
        {
            wallet.UpdateFunds(actionType, request.amount);


        }
        catch (InvalidOperationException ex) when (actionType == ActionType.Add)
        {
            return Result.Failure(WalletErrors.InvalidAmount);
        }
        catch (InvalidOperationException ex) when (actionType == ActionType.Withdraw)
        {
            return Result.Failure(WalletErrors.InsufficientFunds);
        }

        _walletRepository.Update(wallet);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();


    }
}
