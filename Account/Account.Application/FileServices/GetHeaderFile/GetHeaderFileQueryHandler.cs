using Shared.Primitives.Query;

namespace Account.Application.FileServices.GetHeaderFile
{
    internal sealed class GetHeaderFileQueryHandler(IHeaderStorageRepository repository)
        : IQueryRequestHandler<GetHeaderFileQuery, Stream?>
    {
        private readonly IHeaderStorageRepository _repository = repository;

        public Task<Stream?> Handle(GetHeaderFileQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetHeaderAsync(request.UserId, cancellationToken);
        }
    }
}
