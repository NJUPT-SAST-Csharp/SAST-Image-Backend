using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.UpdateAlbumInfo
{
    public sealed class UpdateAlbumInfoCommand(
        long albumId,
        string title,
        string description,
        long categoryId,
        Accessibility accessibility,
        ClaimsPrincipal user
    ) : ICommand
    {
        public long AlbumId { get; } = albumId;
        public string Title { get; } = title;
        public string Description { get; } = description;
        public long CategoryId { get; } = categoryId;
        public Accessibility Accessibility { get; } = accessibility;
        public RequesterInfo Requester { get; } = new(user);
    }
}
