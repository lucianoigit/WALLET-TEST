using DOMAIN.Wallets;

namespace INFRAESTRUCTURE.Repositories;

internal sealed class WalletRepository : Repository<Wallet, int>, IWalletRepository
{
    public WalletRepository(ApplicationDbContext context) : base(context)
    {
    }
}
