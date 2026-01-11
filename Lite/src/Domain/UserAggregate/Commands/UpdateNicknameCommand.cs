using Domain.Shared;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record class UpdateNicknameCommand(Nickname Nickname, Actor Actor) : ICommand { }

internal sealed class UpdateNicknameCommandHandler(IUserRepository repository)
    : ICommandHandler<UpdateNicknameCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateNicknameCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.UpdateNickname(command);

        return Unit.Value;
    }
}
