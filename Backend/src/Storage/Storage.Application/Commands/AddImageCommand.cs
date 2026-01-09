using System.Diagnostics.CodeAnalysis;
using Mediator;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Application.Commands;

public readonly record struct AddImageResult(
    IFileToken? Token,
    [property: MemberNotNullWhen(true, nameof(AddImageResult.Token))] bool Success
)
{
    public static readonly AddImageResult Fail = new(null, false);
}

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
        // TODO: What if S3 add suceeds but Cache insert fails? Should we have a transaction here?

        var token = await storage.AddAsync(command.File, command.BucketName, cancellationToken);

        if (token is null)
            return AddImageResult.Fail;

        await repository.AddAsync(token, cancellationToken);

        return new(token, true);
    }
}
