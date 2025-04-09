using APPLICATION.Abstractions.Data;
using APPLICATION.Features.Transfers.Wallets.UpdateFunds;
using DOMAIN.Wallets;
using Moq;
using Xunit;

namespace TEST.Wallets;

public class UpdateFundsCommandHandlerTests
{
    private readonly Mock<IWalletRepository> _walletRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdateFundsCommandHandler _handler;

    public UpdateFundsCommandHandlerTests()
    {
        _handler = new UpdateFundsCommandHandler(_unitOfWorkMock.Object, _walletRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidActionType_WhenActionIsInvalid()
    {
        var command = new UpdateFundsCommand(1, "invalid", 100);

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.InvalidActionType, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnWalletNotFound_WhenWalletDoesNotExist()
    {
        _walletRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Wallet?)null);

        var command = new UpdateFundsCommand(1, "Add", 100);

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.WalletNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidAmount_WhenAddingNegativeAmount()
    {
        var wallet = Wallet.Create("123", "Test Wallet", 100);
        _walletRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(wallet);

        var command = new UpdateFundsCommand(wallet.Id, "Add", -50);

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.InvalidAmount, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnInsufficientFunds_WhenWithdrawingMoreThanBalance()
    {
        var wallet = Wallet.Create("123", "Test Wallet", 50);
        _walletRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(wallet);

        var command = new UpdateFundsCommand(wallet.Id, "Withdraw", 100);

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.InsufficientFunds, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenAddingValidAmount()
    {
        var wallet = Wallet.Create("123", "Test Wallet", 50);
        _walletRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(wallet);

        var command = new UpdateFundsCommand(wallet.Id, "Add", 100);

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        _walletRepoMock.Verify(r => r.Update(wallet), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenWithdrawingValidAmount()
    {
        var wallet = Wallet.Create("123", "Test Wallet", 200);
        _walletRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(wallet);

        var command = new UpdateFundsCommand(wallet.Id, "Withdraw", 100);

        var result = await _handler.Handle(command, default);

        Assert.True(result.IsSuccess);
        _walletRepoMock.Verify(r => r.Update(wallet), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}