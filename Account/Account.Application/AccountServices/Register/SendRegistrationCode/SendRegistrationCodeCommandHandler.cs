using Account.Application.Services;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;

public sealed class SendRegistrationCodeCommandHandler(
    IAuthCodeCache cache,
    IAuthCodeSender sender,
    ILogger<SendRegistrationCodeCommandHandler> logger
) : ICommandHandler<SendRegistrationCodeCommand>
{
    public async ValueTask<Unit> Handle(
        SendRegistrationCodeCommand request,
        CancellationToken cancellationToken
    )
    {
        int code = Random.Shared.Next(100000, 999999);

        await cache.StoreCodeAsync(
            CodeCacheKey.Registration,
            request.Email,
            code,
            cancellationToken
        );

        logger.LogInformation("Code [{code}] for [{email}] has been stored", code, request.Email);

        await sender.SendCodeAsync(request.Email, code, cancellationToken);

        return Unit.Value;
    }
}
