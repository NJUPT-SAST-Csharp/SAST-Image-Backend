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
        ILogger<LoginEndpointHandler> logger,
        IJwtProvider jwt,
        IUnitOfWork unit
    ) : IEndpointHandler<LoginRequest>
    {
        private readonly IJwtProvider _jwt = jwt;
        private readonly IUserQueryRepository _repository = repository;
        private readonly ILogger<LoginEndpointHandler> _logger = logger;
        private readonly IUnitOfWork _unit = unit;

        public async Task<IResult
        //Results<Ok<DataResponse<LoginDto>>, BadRequest<BadRequestResponse>>
        > Handle(LoginRequest request)
        {
            if (await _repository.GetUserByUsernameAsync(request.Username) is not { } user)
            {
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }

            bool result = user.Login(request.Password);

            if (result == false)
            {
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }

            _logger.LogInformation("User [{username}] logged in.", request.Username);

            var jwt = _jwt.GetLoginJwt(
                user.Id.ToString(),
                user.Username,
                user.Roles.Select(r => r.Name)
            );

            _ = _unit.SaveChangesAsync();

            return Responses.Data(new LoginDto(jwt));
        }
    }
}
