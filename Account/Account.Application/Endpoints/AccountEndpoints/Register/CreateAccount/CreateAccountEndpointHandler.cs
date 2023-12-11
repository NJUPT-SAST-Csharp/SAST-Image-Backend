using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity;
using Account.Entity.UserEntity.Repositories;
using Auth.Authentication;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountEndpointHandler(
        IPasswordHasher hasher,
        IUserCommandRepository repository,
        IUserCheckRepository checker
    ) : IAuthEndpointHandler<CreateAccountRequest>
    {
        private readonly IUserCheckRepository _checker = checker;
        private readonly IUserCommandRepository _repository = repository;
        private readonly IPasswordHasher _hasher = hasher;

        public async Task<IResult> Handle(CreateAccountRequest request, ClaimsPrincipal user)
        {
            var isValid = user.TryFetchEmail(out var email);
            if (isValid == false || await _checker.CheckEmailExistenceAsync(email!))
            {
                return Responses.ValidationFailure("Email", "Duplicated email");
            }

            var hash = await _hasher.HashAsync(request.Password);
            User newAccount = new(request.Username, hash, email!);
            var result = await _repository.CreateUserAsync(newAccount);

            if (result == false)
            {
                return Responses.BadRequest("Something went wrong.");
            }

            return Responses.NoContent;
        }
    }
}
