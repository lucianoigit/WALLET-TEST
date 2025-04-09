using DOMAIN.SharedKernel.Abstractions;

namespace DOMAIN.Wallets;

public interface IWalletRepository:IRepository<Wallet,int>
{
    Task<bool> ExistsAsync(string documentId, CancellationToken cancellationToken = default);

    Task<List<Wallet>?> GetAllAsync(CancellationToken cancellationToken);
}
