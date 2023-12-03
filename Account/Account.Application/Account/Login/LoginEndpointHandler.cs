using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.User.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Account.Login
{
    public sealed class LoginEndpointHandler(
        IUserCheckRepository repository,
        IPasswordHasher passwordHasher
    ) : IEndpointHandler<LoginRequest>
    {
        private readonly IUserCheckRepository _repository = repository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<IResult
        //Results<Ok<DataResponse<LoginDto>>, BadRequest<BadRequestResponse>>
        > Handle(LoginRequest request)
        {
            var passwordHash = await _passwordHasher.HashAsync(request.Password);
            var isValid = await _repository.CheckSignInAsync(request.Username, passwordHash);
            if (isValid)
            {
                return Responses.Data(new LoginDto("2333"));
            }
            else
            {
                return Responses.BadRequest("Login failed.", "Username or password is incorrect.");
            }
        }
    }
}
