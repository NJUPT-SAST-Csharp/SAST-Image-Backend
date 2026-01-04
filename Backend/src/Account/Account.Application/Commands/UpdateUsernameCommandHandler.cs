using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.Services;
using Mediator;

namespace Account.Application.Commands;

internal sealed class UpdateUsernameCommandHandler(IUserRepository repository)
    : ICommandHandler<UpdateUsernameCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateUsernameCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetByIdAsync(command.Requester.Id, cancellationToken);

        user.UpdateUsername(command);

        return Unit.Value;
    }
}
