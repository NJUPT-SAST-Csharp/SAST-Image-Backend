using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity;
using Account.Entity.UserEntity.Repositories;
using Messenger;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountEndpointHandler(
        IAuthCodeCache cache,
        IUserCommandRepository repository,
        IMessagePublisher messenger,
        IJwtProvider provider
    ) : IEndpointHandler<CreateAccountRequest>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IUserCommandRepository _repository = repository;
        private readonly IMessagePublisher _messenger = messenger;
        private readonly IJwtProvider _provider = provider;

        public async Task<IResult> Handle(CreateAccountRequest request)
        {
            User newAccount = new(request.Username, request.Password, request.Email);

            await _repository.CreateUserAsync(newAccount);

            await _cache.DeleteCodeAsync(CodeCacheKey.Registration, request.Email);

            await _messenger.PublishAsync(
                "account.user.created",
                new UserCreatedMessage(newAccount.Id)
            );

            var jwt = _provider.GetLoginJwt(
                newAccount.Id.ToString(),
                newAccount.Username,
                newAccount.Roles.Select(r => r.Name).ToArray()
            );

            return Responses.Data<CreateAccountDto>(new(jwt));
        }
    }
}
