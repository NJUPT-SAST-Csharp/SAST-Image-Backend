using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.User.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Account.Login
{
    public sealed class LoginEndpointHandler(
        IUserCheckRepository repository,
        IPasswordHasher passwordHasher,
        ILogger<LoginEndpointHandler> logger
    ) : IEndpointHandler<LoginRequest>
    {
        private readonly IUserCheckRepository _repository = repository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ILogger<LoginEndpointHandler> _logger = logger;

        public async Task<IResult
        //Results<Ok<DataResponse<LoginDto>>, BadRequest<BadRequestResponse>>
        > Handle(LoginRequest request)
        {
            var passwordHash = await _passwordHasher.HashAsync(request.Password);
            var isValid = await _repository.CheckSignInAsync(request.Username, passwordHash);
            if (isValid)
            {
                _logger.LogInformation("User \"{username}\" logged in.", request.Username);
                return Responses.Data(new LoginDto("2333"));
            }
            else
            {
                _logger.LogInformation(
                    "Tried to login with incorrect username \"{username}\" and password \"{password}\"",
                    request.Username,
                    request.Password
                );
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }
        }
    }
}
