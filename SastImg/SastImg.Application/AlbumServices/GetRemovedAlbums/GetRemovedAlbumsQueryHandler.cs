using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public sealed class GetRemovedAlbumsQueryHandler(IGetRemovedAlbumsRepository repository)
        : IQueryRequestHandler<GetRemovedAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IGetRemovedAlbumsRepository _repository = repository;

        public Task<IEnumerable<AlbumDto>> Handle(
            GetRemovedAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                UserId id = request.AuthorId.Value == 0 ? request.Requester.Id : request.AuthorId;
                return _repository.GetAlbumsByAdminAsync(id, cancellationToken);
            }
            else
            {
                return _repository.GetAlbumsByUserAsync(request.Requester.Id, cancellationToken);
            }
        }
    }
}
