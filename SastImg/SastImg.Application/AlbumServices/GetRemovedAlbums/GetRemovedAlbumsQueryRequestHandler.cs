﻿using SastImg.Application.AlbumServices.GetAlbums;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public sealed class GetRemovedAlbumsQueryRequestHandler(IGetRemovedAlbumsRepository repository)
        : IQueryRequestHandler<GetRemovedAlbumsQueryRequest, IEnumerable<AlbumDto>>
    {
        private readonly IGetRemovedAlbumsRepository _repository = repository;

        public Task<IEnumerable<AlbumDto>> Handle(
            GetRemovedAlbumsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                long id = request.AuthorId == 0 ? request.Requester.Id : request.AuthorId;
                return _repository.GetAlbumsByAdminAsync(id, cancellationToken);
            }
            else
            {
                return _repository.GetAlbumsByUserAsync(request.Requester.Id, cancellationToken);
            }
        }
    }
}
