using Grpc.Core;
using Mediator;
using Storage.Application.Commands;

namespace Storage.WebAPI.Endpoint;

public sealed class ConfirmGrpcService(IMediator mediator) : Contract.ContractBase
{
    public override async Task<ConfirmReply> Confirm(
        ConfirmRequest request,
        ServerCallContext context
    )
    {
        var result = await mediator.Send(
            new ConfirmCommand(request.Token),
            context.CancellationToken
        );

        return new() { Success = result.Success };
    }
}
