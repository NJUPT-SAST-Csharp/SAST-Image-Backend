using Domain.Shared;
using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public record ResetPasswordCommand(
    PasswordInput OldPassword,
    PasswordInput NewPassword,
    Actor Actor
) : ICommand;

internal sealed class ResetPasswordCommandHandler(
    IUserRepository repository,
    IPasswordValidator validator,
    IPasswordGenerator generator
) : ICommandHandler<ResetPasswordCommand>
{
    public async ValueTask<Unit> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        await user.ResetPasswordAsync(command, validator, generator, cancellationToken);

        return Unit.Value;
    }
}
