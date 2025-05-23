﻿using APPLICATION.Features.Transfers.Wallets.CreateWallet;
using APPLICATION.Features.Transfers.Wallets.GetAllWallet;
using APPLICATION.Features.Transfers.Wallets.UpdateFunds;
using APPLICATION.Features.Transfers.Wallets.UpdateWallet;
using Carter;
using DOMAIN.SharedKernel.Primitives;
using MediatR;
using System.Reflection;
using System.Threading;

namespace API.Routes.Wallets;

public class WalletEndpoints:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder routes)
    {
        var wallet = routes.MapGroup("/api/wallet/")
            .WithTags("wallets")
            .WithOpenApi();

        wallet.MapPost("/POST", CreteWallet);
        wallet.MapPut("/PUT/{id}", UpdateWallet);
        wallet.MapPut("/UpdateFunds/{id}", UpdateFunds);
        wallet.MapGet("/GET", GetAllWallet);


    }

    private static async Task<IResult> GetAllWallet
    (
        int quantity,
        int page,
        ISender sender,
        CancellationToken cancellationToken)
    {
            var query = new GetAllWalletQuery(quantity, page);

            var result = await sender.Send(query, cancellationToken);

            return TypedResults.Ok(result);
        }

        private static async Task<IResult> UpdateFunds(
        int id,
        UpdateFundsRequest data,
        ISender sender,
        CancellationToken cancellationToken)

    {
        var command = new UpdateFundsCommand(id, data.action, data.amount);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure) return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result);
    }

    private static async Task<IResult> UpdateWallet(
        int id,
        UpdateWalletRequest data,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateWalletCommand(id,data.name);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure) return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result);

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
