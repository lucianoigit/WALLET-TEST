using APPLICATION.Abstractions.Messagin;
using APPLICATION.Helpers; 

namespace APPLICATION.Features.Transfers.Wallets.GetAllWallet;

public sealed record GetAllWalletQuery(int offset, int take) : IQuery<IPagedList<WalletResponse>>;
