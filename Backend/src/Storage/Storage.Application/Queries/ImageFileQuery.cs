using System.IO.Pipelines;
using Mediator;
using Storage.Application.Service;

namespace Storage.Application.Queries;

public sealed record class ImageFileQuery(string Token, PipeWriter Writer) : IQuery<bool>;

internal sealed class ImageFileQueryHandler(IFileStorage storage, ITokenValidator validator)
    : IQueryHandler<ImageFileQuery, bool>
{
    public async ValueTask<bool> Handle(ImageFileQuery query, CancellationToken cancellationToken)
    {
        if (validator.TryValidate(query.Token, out var token) is false)
            return false;

        bool result = await storage.TryWriteAsync(token, query.Writer, cancellationToken);

        return result;
    }
}
