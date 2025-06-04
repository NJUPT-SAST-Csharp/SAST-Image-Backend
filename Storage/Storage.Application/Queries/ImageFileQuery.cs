using Mediator;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Application.Queries;

public sealed record class ImageFileQuery(string Token) : IQuery<IImageFile?>;

internal sealed class ImageFileQueryHandler(IFileStorage storage, ITokenValidator validator)
    : IQueryHandler<ImageFileQuery, IImageFile?>
{
    public async ValueTask<IImageFile?> Handle(
        ImageFileQuery query,
        CancellationToken cancellationToken
    )
    {
        if (validator.TryValidate(query.Token, out var token) is false)
            return null;

        var file = await storage.GetImageAsync(token.Value, cancellationToken);

        return file;
    }
}
