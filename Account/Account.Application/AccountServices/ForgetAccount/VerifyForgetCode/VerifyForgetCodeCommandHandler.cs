using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeCommandHandler(
        IJwtProvider provider,
        IUserRepository repository
    ) : ICommandRequestHandler<VerifyForgetCodeCommand, VerifyForgetCodeDto>
    {
        private readonly IJwtProvider _provider = provider;
        private readonly IUserRepository _repository = repository;

        public async Task<VerifyForgetCodeDto> Handle(
            VerifyForgetCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);

            var jwt = _provider.GetLoginJwt(user.Id, user.Username, user.Roles);

            return new(jwt);
        }
    }
}
