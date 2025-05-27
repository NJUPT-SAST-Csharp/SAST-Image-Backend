using Account.Domain.UserEntity.Services;
using Mediator;
using Primitives;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize;

public sealed class AuthorizeCommandHandler(IUserRepository users, IUnitOfWork unit)
    : ICommandHandler<AuthorizeCommand>
{
    public async ValueTask<Unit> Handle(
        AuthorizeCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await users.GetUserByIdAsync(request.UserId, cancellationToken);

        user.UpdateAuthorizations(request.Roles);

        await unit.CommitChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
