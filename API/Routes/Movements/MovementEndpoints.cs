using APPLICATION.Features.Transfers.Movements.CreateMovement;
using APPLICATION.Features.Transfers.Movements.GetAllMovement;
using Carter;
using DOMAIN.SharedKernel.Primitives;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Routes.Movements;

public class MovementEndpoints:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder routes)
    {
        var movement = routes.MapGroup("/api/movement/")
            .WithTags("movement")
            .WithOpenApi();

        movement.MapPost("/POST", CreateMovement);
        movement.MapGet("/GET", GetAllMovement);

    }

    private static async Task<IResult> GetAllMovement(

        int quantity,
        int page,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllMovementQuery(quantity, page);

       var result = await sender.Send(query, cancellationToken);

       return TypedResults.Ok(result);
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
