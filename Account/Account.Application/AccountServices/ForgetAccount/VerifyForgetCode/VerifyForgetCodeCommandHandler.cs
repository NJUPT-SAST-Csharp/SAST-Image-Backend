using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeCommandHandler(
        IAuthCodeCache cache,
        IJwtProvider provider,
        IUserRepository repository
    ) : ICommandRequestHandler<VerifyForgetCodeCommand, VerifyForgetCodeDto?>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IJwtProvider _provider = provider;
        private readonly IUserRepository _repository = repository;

        public async Task<VerifyForgetCodeDto?> Handle(
            VerifyForgetCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _cache.VerifyCodeAsync(
                CodeCacheKey.ForgetAccount,
                request.Email,
                request.Code,
                cancellationToken
            );

            if (result == false)
            {
                return null;
            }
            var user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);

            var jwt = _provider.GetLoginJwt(user.Id, user.Username, user.Roles);

            return new(jwt);
        }
    }
}
