using Account.Application.Services;
using Microsoft.Extensions.Logging;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public sealed class SendRegistrationCodeCommandHandler(
        IAuthCodeCache cache,
        IAuthCodeSender sender,
        ILogger<SendRegistrationCodeCommandHandler> logger
    ) : ICommandRequestHandler<SendRegistrationCodeCommand>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IAuthCodeSender _sender = sender;
        private readonly ILogger<SendRegistrationCodeCommandHandler> _logger = logger;

        public async Task Handle(
            SendRegistrationCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            int code = Random.Shared.Next(100000, 999999);

            await _cache.StoreCodeAsync(
                CodeCacheKey.Registration,
                request.Email,
                code,
                cancellationToken
            );

            _logger.LogInformation(
                "Code [{code}] for [{email}] has been stored",
                code,
                request.Email
            );

            await _sender.SendCodeAsync(request.Email, code, cancellationToken);
        }
    }
}
