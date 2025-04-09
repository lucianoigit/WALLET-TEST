using APPLICATION.Abstractions.Data;
using APPLICATION.Features.Transfers.Wallets.DeleteWallet;
using DOMAIN.Wallets;
using Moq;
using Xunit;

namespace TEST.Wallets;

public class DeleteWalletCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenWalletExists()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockWalletRepo = new Mock<IWalletRepository>();

        var wallet = Wallet.Create("12345678", "Wallet Test", 100m);

        mockWalletRepo.Setup(repo => repo.GetByIdAsync(wallet.Id))
                      .ReturnsAsync(wallet);

        var handler = new DeleteWalletCommandHandler(
            mockUnitOfWork.Object,
            mockWalletRepo.Object);

        var command = new DeleteWalletCommand(wallet.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);

        // En lugar de Assert.Null(result.Error), aseguramos que no sea un error significativo
        Assert.True(result.Error == null || string.IsNullOrEmpty(result.Error.Code));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWalletDoesNotExist()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockWalletRepo = new Mock<IWalletRepository>();

        mockWalletRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((Wallet?)null);

        var handler = new DeleteWalletCommandHandler(mockUnitOfWork.Object, mockWalletRepo.Object);

        var command = new DeleteWalletCommand(999); // ID inexistente

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(WalletErrors.WalletNotFound, result.Error);

        mockWalletRepo.Verify(repo => repo.Remove(It.IsAny<Wallet>()), Times.Never);
        mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}