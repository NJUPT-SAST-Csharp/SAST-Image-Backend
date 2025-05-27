using Mediator;

namespace Account.Application.FileServices.GetHeaderFile;

public sealed class GetHeaderFileQueryHandler(IHeaderStorageRepository repository)
    : IQueryHandler<GetHeaderFileQuery, Stream?>
{
    public async ValueTask<Stream?> Handle(
        GetHeaderFileQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetHeaderAsync(request.UserId, cancellationToken);
    }
}
