﻿using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.CategoryEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.SearchAlbums
{
    public sealed class SearchAlbumsQuery(
        int categoryId,
        string title,
        int page,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<SearchAlbumDto>>
    {
        public int Page { get; } = page;
        public string Title { get; } = title;
        public CategoryId CategoryId { get; } = new(categoryId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
