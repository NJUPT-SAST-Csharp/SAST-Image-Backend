using Mediator;

namespace Account.Application.FileServices.GetAvatarFile;

public sealed class GetAvatarFileQueryHandler(IAvatarStorageRepository repository)
    : IQueryHandler<GetAvatarFileQuery, Stream?>
{
    private readonly IAvatarStorageRepository _repository = repository;

    public async ValueTask<Stream?> Handle(
        GetAvatarFileQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _repository.GetAvatarAsync(request.UserId, cancellationToken);
    }
}
