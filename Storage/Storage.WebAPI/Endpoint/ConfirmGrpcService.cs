using Grpc.Core;
using Mediator;
using Storage.Application.Commands;
using Storage.Application.Model;

namespace Storage.WebAPI.Endpoint;

public sealed class ConfirmGrpcService(IMediator mediator) : Contract.ContractBase
{
    public override async Task<ConfirmReply> Confirm(
        ConfirmRequest request,
        ServerCallContext context
    )
    {
        if (FileToken.TryParse(request.Token, out var token) is false)
            return new ConfirmReply() { Success = false };

        var result = await mediator.Send(
            new ConfirmCommand(token.Value),
            context.CancellationToken
        );

        return new() { Success = result.Success };
    }
}
