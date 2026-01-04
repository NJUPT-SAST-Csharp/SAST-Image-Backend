using Mediator;
using Storage.Application.Service;

namespace Storage.Application.Commands;

public sealed class DeleteExpiredFilesCommand : ICommand;

internal sealed class CheckTokensValidityCommandHandler(
    ITokenRepository repository,
    IFileStorage storage
) : ICommandHandler<DeleteExpiredFilesCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteExpiredFilesCommand request,
        CancellationToken cancellationToken
    )
    {
        var tokens = await repository.GetExpiredAsync(cancellationToken);

        tokens = await storage.DeleteAsync(tokens, cancellationToken);

        await repository.DeleteAsync(tokens, cancellationToken);

        return Unit.Value;
    }
}
