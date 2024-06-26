﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.TagEntity;

namespace SastImg.Application.ImageServices.AddImage
{
    public sealed class AddImageCommand(
        string title,
        string description,
        long[] tags,
        IFormFile file,
        long albumId,
        ClaimsPrincipal user
    ) : ICommandRequest<ImageInfoDto>
    {
        public IFormFile ImageFile { get; } = file;

        public AlbumId AlbumId { get; } = new(albumId);

        public ImageTitle Title { get; } = new(title);

        public ImageDescription Description { get; } = new(description);

        public TagId[] Tags { get; } = Array.ConvertAll(tags, tag => new TagId(tag));

        public RequesterInfo Requester { get; } = new(user);
    }
}
