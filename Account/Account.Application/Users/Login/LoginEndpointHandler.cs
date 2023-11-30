using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Application.Users.Repository;
using Account.WebAPI.Endpoints.Login;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Users.Login
{
    public sealed class LoginEndpointHandler(
        IUserRepository repository,
        IPasswordHasher passwordHasher,
        IValidator<LoginRequest> validator
    ) : IEndpointHandler
    {
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IValidator<LoginRequest> _validator = validator;

        public async Task<IResult> Handle(LoginRequest request)
        {
            var result = await _validator.ValidateAsync(request);
            if (result.IsValid == false)
                return ResponseBuilder.ValidationFailure(result.ToDictionary());

            var userIdentity = await _repository.GetUserIdentityByUsernameAsync(request.Username);
            if (
                userIdentity is null
                || await _passwordHasher.ValidateAsync(request.Password, userIdentity.PasswordHash)
                    == false
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
