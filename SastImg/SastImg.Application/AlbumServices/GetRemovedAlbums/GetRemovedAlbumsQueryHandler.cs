using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public sealed class GetRemovedAlbumsQueryHandler(IGetRemovedAlbumsRepository repository)
        : IQueryRequestHandler<GetRemovedAlbumsQuery, IEnumerable<RemovedAlbumDto>>
    {
        private readonly IGetRemovedAlbumsRepository _repository = repository;

        public Task<IEnumerable<RemovedAlbumDto>> Handle(
            GetRemovedAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                UserId id = request.AuthorId.Value == 0 ? request.Requester.Id : request.AuthorId;
                return _repository.GetRemovedAlbumsByAdminAsync(id, cancellationToken);
            }
            else
            {
                return _repository.GetRemovedAlbumsByUserAsync(
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
    }
}
