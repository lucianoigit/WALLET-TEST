using APPLICATION.Abstractions.Data;
using APPLICATION.Features.Transfers.Wallets.CreateWallet;
using DOMAIN.Wallets;
using Moq;
using Xunit;
namespace TEST.Wallets;

public class CreateWalletCommandHandlerTest
{

    [Fact]
    public async Task Handle_Should_CreateWallet_When_DocumentIdDoesNotExist()
    {
        // Arrange
        var documentId = "123456789";
        var name = "Test Wallet";
        var balance = 1000m;

        var command = new CreateWalletCommand(documentId, name, balance);

        var walletRepositoryMock = new Mock<IWalletRepository>();
        walletRepositoryMock
            .Setup(r => r.ExistsAsync(documentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var handler = new CreateWalletCommandHandler(unitOfWorkMock.Object, walletRepositoryMock.Object);


        var result = await handler.Handle(command, CancellationToken.None);

     
        Assert.True(result.IsSuccess);
        walletRepositoryMock.Verify(r => r.Add(It.IsAny<Wallet>()), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_WalletAlreadyExists()
    {
        // Arrange
        var documentId = "123456789";
        var name = "Test Wallet";
        var balance = 1000m;

        var command = new CreateWalletCommand(documentId, name, balance);

        var walletRepositoryMock = new Mock<IWalletRepository>();
        walletRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);



        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new CreateWalletCommandHandler(unitOfWorkMock.Object, walletRepositoryMock.Object);

     
        var result = await handler.Handle(command, CancellationToken.None);

      
        Assert.True(result.IsFailure);
        Assert.Equal(WalletErrors.WalletAlreadyExists, result.Error);
        walletRepositoryMock.Verify(r => r.Add(It.IsAny<Wallet>()), Times.Never);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
