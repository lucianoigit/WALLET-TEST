using APPLICATION.Abstractions.Data;
using APPLICATION.Features.Transfers.Wallets.UpdateWallet;
using DOMAIN.Wallets;
using FluentAssertions;
using Moq;
using Xunit;

namespace TEST.Wallets;

public class UpdateWalletCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IWalletRepository> _walletRepositoryMock = new();

    private readonly UpdateWalletCommandHandler _handler;

    public UpdateWalletCommandHandlerTests()
    {
        _handler = new UpdateWalletCommandHandler(
            _unitOfWorkMock.Object,
            _walletRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_WalletNotFound()
    {
        // Arrange
        var command = new UpdateWalletCommand(1, "Updated Name");

        _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Wallet?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WalletErrors.WalletNotFound);
    }

    [Fact]
    public async Task Handle_Should_UpdateWalletAndReturnSuccess_When_WalletExists()
    {
        // Arrange
        var existingWallet = Wallet.Create("12345678", "John", 1000m);

        typeof(Wallet)
            .GetProperty(nameof(Wallet.Id))!
            .SetValue(existingWallet, 1); // Simular ID asignado

        var command = new UpdateWalletCommand(1, "Updated Name");

        _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingWallet);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingWallet.Name.Should().Be("Updated Name");
        _walletRepositoryMock.Verify(r => r.Update(existingWallet), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
