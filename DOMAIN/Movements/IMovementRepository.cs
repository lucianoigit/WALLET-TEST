using DOMAIN.SharedKernel.Abstractions;


namespace DOMAIN.Movements;

public interface IMovementRepository:IRepository<Movement, int>
{
    Task<List<Movement>?> GetAllAsync(CancellationToken cancellationToken);
}
