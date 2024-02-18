using Account.Application.Services;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeCommandHandler(IAuthCodeCache cache)
        : ICommandRequestHandler<VerifyRegistrationCodeCommand, bool>
    {
        private readonly IAuthCodeCache _cache = cache;

        public async Task<bool> Handle(
            VerifyRegistrationCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _cache.VerifyCodeAsync(
                CodeCacheKey.Registration,
                request.Email,
                request.Code,
                cancellationToken
            );

            return result;
        }
    }
}
