using APPLICATION.Features.Transfers.Wallets.CreateWallet;
using Carter;
using DOMAIN.SharedKernel.Primitives;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Routes.Wallets;

public class WalletEndpoints:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder routes)
    {
        var loans = routes.MapGroup("/api/wallet/")
            .WithTags("wallets")
            .WithOpenApi();

        loans.MapPost("/POST", CreteWallet);

    }

    private static async Task<IResult> CreteWallet(
        CreateWalletRequest data,
        ISender sender,
        CancellationToken cancellationToken)
    {

        var command = new CreateWalletCommand(
            data.name,
            data.documentId,
            data.balance);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure) return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result);
    }
}
