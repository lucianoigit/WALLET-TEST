using APPLICATION.Features.Transfers.Movements.CreateMovement;
using Carter;
using DOMAIN.SharedKernel.Primitives;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Routes.Movements;

public class MovementEndpoints:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder routes)
    {
        var loans = routes.MapGroup("/api/movement/")
            .WithTags("movement")
            .WithOpenApi();

        loans.MapPost("/POST", CreateMovement);

    }

    private static async Task<IResult> CreateMovement(
        CreateMovementRequest data,
        ISender sender,
        CancellationToken cancellationToken)
    {

        var command = new CreateMovementCommand(
            data.walletId,
            data.receivingWalletId,
            data.ammount,
            data.type);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure) return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result);
    }
}
