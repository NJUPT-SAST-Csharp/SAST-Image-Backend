using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    public sealed class CreateAlbumCommand(
        string title,
        string description,
        long categoryId,
        int accessibility,
        ClaimsPrincipal user
    ) : ICommand<CreateAlbumDto>
    {
        public string Title { get; } = title;
        public string Description { get; } = description;
        public long CategoryId { get; } = categoryId;
        public Accessibility Accessibility { get; } = (Accessibility)accessibility;
        public RequesterInfo Requester { get; } = new(user);
    }
}
