using Identity;
using Mediator;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.SearchAlbums;

public sealed record class SearchAlbumsQuery(
    CategoryId CategoryId,
    string SearchTitle,
    int Page,
    Requester Requester
) : IQuery<IEnumerable<SearchAlbumDto>>;
