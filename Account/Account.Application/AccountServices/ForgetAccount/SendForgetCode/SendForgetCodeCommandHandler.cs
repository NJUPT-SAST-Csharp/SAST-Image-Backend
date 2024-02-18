using Account.Application.Services;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeCommandHandler(IAuthCodeSender sender, IAuthCodeCache cache)
        : ICommandRequestHandler<SendForgetCodeCommand>
    {
        private readonly IAuthCodeSender _sender = sender;
        private readonly IAuthCodeCache _cache = cache;

        public async Task Handle(SendForgetCodeCommand request, CancellationToken cancellationToken)
        {
            var code = Random.Shared.Next(100000, 999999);
            await _sender.SendCodeAsync(request.Email, code, cancellationToken);
            await _cache.StoreCodeAsync(
                CodeCacheKey.ForgetAccount,
                request.Email,
                code,
                cancellationToken
            );
        }
    }
}
