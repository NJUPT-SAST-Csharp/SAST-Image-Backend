using Domain.Shared;
using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record ResetUsernameCommand(Username Username, Actor Actor) : ICommand;

internal sealed class ResetUsernameCommandHandler(
    IUserRepository repository,
    IUsernameUniquenessChecker checker
) : ICommandHandler<ResetUsernameCommand>
{
    public async ValueTask<Unit> Handle(
        ResetUsernameCommand command,
        CancellationToken cancellationToken
    )
    {
        await checker.CheckAsync(command.Username, cancellationToken);

        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.ResetUsername(command);

        return Unit.Value;
    }
}
