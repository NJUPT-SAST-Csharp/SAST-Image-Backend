using Mediator;
using Storage.Application.Service;

namespace Storage.Application.Commands;

public readonly record struct ConfirmResult(bool Success);

public sealed record class ConfirmCommand(string Token) : ICommand<ConfirmResult>;

internal sealed class ConfirmCommandHandler(ITokenRepository repository, ITokenValidator validator)
    : ICommandHandler<ConfirmCommand, ConfirmResult>
{
    public async ValueTask<ConfirmResult> Handle(
        ConfirmCommand command,
        CancellationToken cancellationToken
    )
    {
        if (validator.TryValidate(command.Token, out var token) is false)
            return new(false);

        bool result = await repository.ConfirmAsync(token.Value, cancellationToken);

        return new(result);
    }
}
