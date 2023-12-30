﻿using System.Security.Claims;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.SearchImages
{
    public sealed class SearchImagesQueryRequest(
        int page,
        SearchOrder order,
        long categoryId,
        long[] tags,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<ImageDto>>
    {
        public int Page { get; } = page;
        public SearchOrder Order { get; } = order;
        public long CategoryId { get; } = categoryId;
        public long[] Tags { get; } = tags;
        public RequesterInfo Requester { get; } = new(user);

        public long[] Ids { get; set; } = [];
    }
}
