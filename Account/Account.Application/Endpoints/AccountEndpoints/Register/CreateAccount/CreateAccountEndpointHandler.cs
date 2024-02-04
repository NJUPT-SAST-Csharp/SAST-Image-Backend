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
        IMessagePublisher messenger
    ) : IEndpointHandler<CreateAccountRequest>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IUserCommandRepository _repository = repository;
        private readonly IMessagePublisher _messenger = messenger;

        public async Task<IResult> Handle(CreateAccountRequest request)
        {
            User newAccount = new(request.Username, request.Password, request.Email);

            var result = await _repository.CreateUserAsync(newAccount);

            if (result == false)
            {
                return Responses.BadRequest("Something went wrong.");
            }

            await _cache.DeleteCodeAsync(CodeCacheKey.Registration, request.Email);

            await _messenger.PublishAsync(
                "account.user.created",
                new UserCreatedMessage(newAccount.Id)
            );

            return Responses.NoContent;
        }
    }
}
