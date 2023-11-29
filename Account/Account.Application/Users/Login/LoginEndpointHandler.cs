using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Application.Users.Repository;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Users.Login
{
    public sealed class LoginEndpointHandler(
        IUserRepository _repository,
        IPasswordHasher _passwordHasher
    ) : IEndpointHandler
    {
        public async Task<IResult> Handle(string username, string password)
        {
            var userIdentity = await _repository.GetUserIdentityByUsernameAsync(username);
            if (
                userIdentity is null
                || await _passwordHasher.ValidateAsync(password, userIdentity.PasswordHash) == false
            )
            {
                return ResponseBuilder.BadRequest(
                    "Login failed.",
                    "Username or password is incorrect."
                );
            }
            return ResponseBuilder.Data("jwt", "23333333");
        }
    }
}
