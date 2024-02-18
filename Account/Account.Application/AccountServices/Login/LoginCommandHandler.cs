using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginCommandHandler(
        IUserRepository repository,
        IJwtProvider jwtProvider,
        IUnitOfWork unit
    ) : ICommandRequestHandler<LoginCommand, LoginDto>
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unit = unit;

        public async Task<LoginDto> Handle(
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _repository.GetUserByUsernameAsync(
                request.Username,
                cancellationToken
            );

            await user.LoginAsync(request.Password);

            var jwt = _jwtProvider.GetLoginJwt(user.Id, user.Username, user.Roles);

            await _unit.CommitChangesAsync(cancellationToken);

            return new(jwt);
        }
    }
}
