﻿using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    public sealed class CreateAlbumCommand(
        string title,
        string description,
        int categoryId,
        Accessibility accessibility,
        ClaimsPrincipal user
    ) : ICommandRequest<CreateAlbumDto>
    {
        public AlbumTitle Title { get; } = new(title);
        public AlbumDescription Description { get; } = new(description);
        public CategoryId CategoryId { get; } = new(categoryId);
        public Accessibility Accessibility { get; } = accessibility;
        public RequesterInfo Requester { get; } = new(user);
    }
}
