using Mediator;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Application.Commands;

public readonly record struct ConfirmResult(bool Success);

public sealed record class ConfirmCommand(FileToken Token) : ICommand<ConfirmResult>;

internal sealed class ConfirmCommandHandler(ITokenRepository repository)
    : ICommandHandler<ConfirmCommand, ConfirmResult>
{
    public async ValueTask<ConfirmResult> Handle(
        ConfirmCommand command,
        CancellationToken cancellationToken
    )
    {
        await repository.ConfirmAsync(command.Token, cancellationToken);

        return new(true);
    }
}
