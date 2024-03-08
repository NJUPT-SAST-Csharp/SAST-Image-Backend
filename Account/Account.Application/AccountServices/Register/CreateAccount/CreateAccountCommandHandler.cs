using Account.Application.Services;
using Account.Domain.UserEntity;
using Account.Domain.UserEntity.Services;
using Microsoft.AspNetCore.Http;
using Primitives;
using Primitives.Command;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountCommandHandler(
        IAuthCodeCache cache,
        IUserRepository repository,
        IJwtProvider provider,
        IUnitOfWork unit
    ) : ICommandRequestHandler<CreateAccountCommand, IResult>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IUserRepository _repository = repository;
        private readonly IJwtProvider _provider = provider;
        private readonly IUnitOfWork _unit = unit;

        public async Task<IResult> Handle(
            CreateAccountCommand request,
            CancellationToken cancellationToken
        )
        {
            User user = User.CreateNewUser(request.Username, request.Password, request.Email);

            await _repository.AddNewUserAsync(user, cancellationToken);

            var jwt = _provider.GetLoginJwt(user.Id, user.Username, user.Roles);

            await _unit.CommitChangesAsync(cancellationToken);

            await _cache.DeleteCodeAsync(
                CodeCacheKey.Registration,
                request.Email,
                cancellationToken
            );

            return Responses.Data(new CreateAccountDto(jwt));
        }
    }
}
