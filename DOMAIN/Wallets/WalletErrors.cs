using DOMAIN.SharedKernel.Primitives;

namespace DOMAIN.Wallets;

public static class WalletErrors
{
    public static Error WalletNotFound = Error.NotFound("Wallet.NotFound", "The wallet was not found.");
    public static Error WalletAlreadyExists = Error.Conflict("Wallet.AlreadyExists", "A wallet already exists for the given document.");
    public static Error ReceivingWalletNotFound = Error.NotFound("Wallet.ReceivingNotFound", "The receiving wallet does not exist.");
    public static Error IssuingWalletNotFound = Error.NotFound("Wallet.IssuingWalletNotFound", "The Issuing wallet does not exist.");
    public static Error InvalidOperation = Error.NotFound("Wallet.InvalidOperation", "Insufficient funds");
    public static Error InvalidActionType = Error.Validation("Wallet.InvalidActionType", "The selected action does not exist");
    public static Error InvalidAmount = Error.Validation("Wallet.InvalidAmount", "The amount entered is not correct, it must be greater than 0");
    public static Error InsufficientFunds = Error.Validation("Wallet.InsufficientFunds", "Insufficient funds.");


}
