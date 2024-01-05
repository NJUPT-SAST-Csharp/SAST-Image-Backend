using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;

namespace SastImg.Application.AlbumServices.UpdateAlbumInfo
{
    public sealed class UpdateAlbumInfoCommand(
        string title,
        string description,
        ClaimsPrincipal user
    ) : ICommand
    {
        public long AlbumId { get; }
        public string Title { get; } = title;
        public string Description { get; } = description;
        public RequesterInfo Requester { get; } = new(user);
    }
}
