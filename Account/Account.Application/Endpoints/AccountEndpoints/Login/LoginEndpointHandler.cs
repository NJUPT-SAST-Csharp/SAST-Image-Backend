using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginEndpointHandler(
        IUserQueryRepository repository,
        IPasswordHasher passwordHasher,
        ILogger<LoginEndpointHandler> logger,
        IJwtProvider jwt
    ) : IEndpointHandler<LoginRequest>
    {
        private readonly IJwtProvider _jwt = jwt;
        private readonly IUserQueryRepository _repository = repository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ILogger<LoginEndpointHandler> _logger = logger;

        public async Task<IResult
        //Results<Ok<DataResponse<LoginDto>>, BadRequest<BadRequestResponse>>
        > Handle(LoginRequest request)
        {
            if (await _repository.GetUserByUsernameAsync(request.Username) is not { } user)
            {
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }

            var isValid = await _passwordHasher.ValidateAsync(request.Password, user.PasswordHash);

            if (isValid == false)
            {
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }

            _logger.LogInformation("User [{username}] logged in.", request.Username);
            var jwt = _jwt.GetLoginJwt(user.Id.ToString(), user.Username);
            return Responses.Data(new LoginDto(jwt));
        }
    }
}
