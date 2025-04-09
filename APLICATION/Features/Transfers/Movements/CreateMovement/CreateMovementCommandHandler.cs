using APPLICATION.Abstractions.Data;
using APPLICATION.Abstractions.Messagin;
using DOMAIN.Movements;
using DOMAIN.SharedKernel.Primitives;
using DOMAIN.Wallets;

namespace APPLICATION.Features.Transfers.Movements.CreateMovement;

public sealed class CreateMovementCommandHandler : ICommandHandler<CreateMovementCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMovementRepository _movementRepository;
    private readonly IWalletRepository _walletRepository;

    public CreateMovementCommandHandler(
        IUnitOfWork unitOfWork,
        IMovementRepository movementRepository,
        IWalletRepository walletRepository)
    {
        _unitOfWork = unitOfWork;
        _movementRepository = movementRepository;
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(CreateMovementCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<MovementType>(request.type, ignoreCase: true, out var movementType))
        {
            return Result.Failure(MovementErrors.InvalidMovementType);
        }

        var issuingWallet = await _walletRepository.GetByIdAsync(request.walletId);

        if (issuingWallet is null) return Result.Failure(WalletErrors.IssuingWalletNotFound);

        var receivingWallet = await _walletRepository.GetByIdAsync(request.receivingWalletId);

        if (receivingWallet is null) return Result.Failure(WalletErrors.ReceivingWalletNotFound);



        var movement = Movement.Create(
            request.walletId,
            request.receivingWalletId,
            request.ammount,
            movementType);

        try
        {
            issuingWallet.Transfer(movement);
            receivingWallet.Receive(movement);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(WalletErrors.InvalidOperation);
        }

        _movementRepository.Add(movement);

        _walletRepository.Update(issuingWallet);  // I recommend using a repository method called updateAll

        _walletRepository.Update(receivingWallet);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
