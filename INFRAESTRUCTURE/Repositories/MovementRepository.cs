using DOMAIN.Movements;
using DOMAIN.Wallets;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE.Repositories;

internal sealed class MovementRepository : Repository<Movement, int>, IMovementRepository
{
    public MovementRepository(ApplicationDbContext context) : base(context)
    {


    }

    public async Task<List<Movement>?> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Movements
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
