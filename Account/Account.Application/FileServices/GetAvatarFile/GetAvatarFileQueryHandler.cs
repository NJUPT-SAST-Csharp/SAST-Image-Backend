using Shared.Primitives.Query;

namespace Account.Application.FileServices.GetAvatarFile
{
    internal sealed class GetAvatarFileQueryHandler(IAvatarStorageRepository repository)
        : IQueryRequestHandler<GetAvatarFileQuery, Stream?>
    {
        private readonly IAvatarStorageRepository _repository = repository;

        public Task<Stream?> Handle(GetAvatarFileQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAvatarAsync(request.UserId, cancellationToken);
        }
    }
}
