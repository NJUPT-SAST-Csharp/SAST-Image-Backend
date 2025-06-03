using Mediator;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Application.Commands;

public readonly record struct AddImageResult(FileToken Token);

public sealed record class AddImageCommand(IImageFile File, string BucketName)
    : ICommand<AddImageResult>;

internal sealed class AddImageCommandHandler(IFileStorage storage, ITokenRepository repository)
    : ICommandHandler<AddImageCommand, AddImageResult>
{
    public async ValueTask<AddImageResult> Handle(
        AddImageCommand command,
        CancellationToken cancellationToken
    )
    {
        var token = await storage.AddAsync(command.File, command.BucketName, cancellationToken);

        await repository.InsertAsync(token, cancellationToken);

        return new(token);
    }
}
