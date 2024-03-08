using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Microsoft.AspNetCore.Http;
using Primitives;
using Primitives.Command;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginCommandHandler(
        IUserRepository repository,
        IJwtProvider jwtProvider,
        IUnitOfWork unit
    ) : ICommandRequestHandler<LoginCommand, IResult>
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unit = unit;

        public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByUsernameAsync(
                request.Username,
                cancellationToken
            );

            var isValid = await user.LoginAsync(request.Password);

            if (isValid == false)
            {
                return Responses.BadRequest("Login failed", "Username or password is incorrect.");
            }

            var jwt = _jwtProvider.GetLoginJwt(user.Id, user.Username, user.Roles);

            await _unit.CommitChangesAsync(cancellationToken);

            return Responses.Data(new LoginDto(jwt));
        }
    }
}
