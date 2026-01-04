using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.Services;
using Mediator;

namespace Account.Application.Commands;

public sealed class AuthorizeCommandHandler(IUserRepository users)
    : ICommandHandler<AuthorizeCommand>
{
    public async ValueTask<Unit> Handle(
        AuthorizeCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await users.GetByIdAsync(request.UserId, cancellationToken);

        user.Authorize(request);

        return Unit.Value;
    }
}
