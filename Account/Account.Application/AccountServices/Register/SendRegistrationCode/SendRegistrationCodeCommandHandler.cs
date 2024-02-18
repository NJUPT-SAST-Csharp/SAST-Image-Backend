using Account.Application.Services;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public sealed class SendRegistrationCodeCommandHandler(
        IAuthCodeCache cache,
        IAuthCodeSender sender
    ) : ICommandRequestHandler<SendRegistrationCodeCommand>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IAuthCodeSender _sender = sender;

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

            await _sender.SendCodeAsync(request.Email, code, cancellationToken);
        }
    }
}
