using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.RemoveAlbum
{
    public sealed class RemoveAlbumCommand(long albumId, ClaimsPrincipal user) : ICommandRequest
    {
        public AlbumId AlbumId { get; } = new(albumId);
        public RequesterInfo RequesterInfo { get; } = new(user);
    }
}
