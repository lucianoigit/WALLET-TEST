using DOMAIN.Movements;

namespace INFRAESTRUCTURE.Repositories;

internal sealed class MovementRepository : Repository<Movement, int>, IMovementRepository
{
    public MovementRepository(ApplicationDbContext context) : base(context)
    {
    }
}
