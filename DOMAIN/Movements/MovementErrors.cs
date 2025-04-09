using DOMAIN.SharedKernel.Primitives;

namespace DOMAIN.Movements;

public static class MovementErrors
{
    public static Error InvalidMovementType = Error.NotFound("Wallet.InvalidMovementType", "Invalid transfer method.");
}
