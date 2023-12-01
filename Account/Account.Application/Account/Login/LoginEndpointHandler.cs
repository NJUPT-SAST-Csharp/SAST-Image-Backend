using Account.Application.Account.Repository;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.WebAPI.Endpoints.Login;
using Microsoft.AspNetCore.Http.HttpResults;
using Response.ReponseObjects;
using Shared.Response.Builders;

namespace Account.Application.Account.Login
{
    public sealed class LoginEndpointHandler(
        IUserRepository repository,
        IPasswordHasher passwordHasher
    ) : IEndpointHandler
    {
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<
            Results<Ok<DataResponse<LoginDto>>, BadRequest<BadRequestResponse>>
        > Handle(LoginRequest request)
        {
            var userIdentity = await _repository.GetUserIdentityByUsernameAsync(request.Username);
            if (
                userIdentity is null
                || await _passwordHasher.ValidateAsync(request.Password, userIdentity.PasswordHash)
                    == false
            )
            {
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }

            return Responses.Data(new LoginDto("2333"));
        }
    }
}
