using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.User;
using Account.Entity.User.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Account.Register.CreateAccount
{
    public sealed class CreateAccountEndpointHandler(
        IAuthCache cache,
        IPasswordHasher hasher,
        IUserCommandRepository repository
    ) : IEndpointHandler<CreateAccountRequest>
    {
        private readonly IUserCommandRepository _repository = repository;
        private readonly IAuthCache _cache = cache;
        private readonly IPasswordHasher _hasher = hasher;

        public async Task<IResult> Handle(CreateAccountRequest request)
        {
            var hash = await _hasher.HashAsync(request.Password);
            User newAccount = new(request.Username, hash, request.Email);
            var result = await _repository.CreateUserAsync(newAccount);
            if (result)
            {
                return Responses.NoContent;
            }
            else
            {
                return Responses.BadRequest("Something went wrong.");
            }
        }
    }
}
