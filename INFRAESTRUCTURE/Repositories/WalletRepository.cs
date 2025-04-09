using DOMAIN.Wallets;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE.Repositories;

internal sealed class WalletRepository : Repository<Wallet, int>, IWalletRepository
{
    public WalletRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsAsync(string documentId, CancellationToken cancellationToken = default)
    {
        return await _context.Wallets
            .AnyAsync(w => w.DocumentId == documentId, cancellationToken);
    }
}
