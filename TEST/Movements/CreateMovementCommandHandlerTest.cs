using APPLICATION.Abstractions.Data;
using APPLICATION.Features.Transfers.Movements.CreateMovement;
using DOMAIN.Movements;
using DOMAIN.SharedKernel.Primitives;
using DOMAIN.Wallets;
using Moq;
using Xunit;

namespace TEST.Movements;

public class CreateMovementCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMovementRepository> _movementRepositoryMock = new();
    private readonly Mock<IWalletRepository> _walletRepositoryMock = new();

    private CreateMovementCommandHandler CreateHandler() =>
        new(_unitOfWorkMock.Object, _movementRepositoryMock.Object, _walletRepositoryMock.Object);


    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_ValidTransfer()
    {
        // Arrange
        var walletFrom = Wallet.Create("123", "Wallet A", 1000);
        var walletTo = Wallet.Create("456", "Wallet B", 500);

        // Asignamos IDs únicos para distinguir en los mocks
        walletFrom.GetType().GetProperty("Id")!.SetValue(walletFrom, 1);
        walletTo.GetType().GetProperty("Id")!.SetValue(walletTo, 2);

        var command = new CreateMovementCommand(walletFrom.Id, walletTo.Id, 200, "Debit");

        _walletRepositoryMock.Setup(r => r.GetByIdAsync(walletFrom.Id)).ReturnsAsync(walletFrom);
        _walletRepositoryMock.Setup(r => r.GetByIdAsync(walletTo.Id)).ReturnsAsync(walletTo);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _movementRepositoryMock.Verify(r => r.Add(It.IsAny<Movement>()), Times.Once);
        _walletRepositoryMock.Verify(r => r.Update(walletFrom), Times.Once);
        _walletRepositoryMock.Verify(r => r.Update(walletTo), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_InvalidMovementType()
    {
        var command = new CreateMovementCommand(1, 2, 100, "InvalidType");
        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(MovementErrors.InvalidMovementType, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_IssuingWalletNotFound()
    {
        var command = new CreateMovementCommand(1, 2, 100, "Debit");

        _walletRepositoryMock.Setup(r => r.GetByIdAsync(command.walletId)).ReturnsAsync((Wallet?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.IssuingWalletNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_ReceivingWalletNotFound()
    {
        var walletFrom = Wallet.Create("123", "Wallet A", 1000);
        var command = new CreateMovementCommand(walletFrom.Id, 99, 100, "Debit");

        _walletRepositoryMock.Setup(r => r.GetByIdAsync(walletFrom.Id)).ReturnsAsync(walletFrom);
        _walletRepositoryMock.Setup(r => r.GetByIdAsync(command.receivingWalletId)).ReturnsAsync((Wallet?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.ReceivingWalletNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_InvalidOperation()
    {
        var walletFrom = Wallet.Create("123", "Wallet A", 50); // saldo insuficiente
        var walletTo = Wallet.Create("456", "Wallet B", 500);

        // Asignamos IDs únicos para distinguir en los mocks
        walletFrom.GetType().GetProperty("Id")!.SetValue(walletFrom, 1);
        walletTo.GetType().GetProperty("Id")!.SetValue(walletTo, 2);
        var command = new CreateMovementCommand(walletFrom.Id, walletTo.Id, 100, "Debit");

        _walletRepositoryMock.Setup(r => r.GetByIdAsync(walletFrom.Id)).ReturnsAsync(walletFrom);
        _walletRepositoryMock.Setup(r => r.GetByIdAsync(walletTo.Id)).ReturnsAsync(walletTo);

        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.InvalidOperation, result.Error);
    }
}
