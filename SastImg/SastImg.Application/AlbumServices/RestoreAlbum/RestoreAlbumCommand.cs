using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.RestoreAlbum
{
    public sealed class RestoreAlbumCommand(long albumId, ClaimsPrincipal user) : ICommandRequest
    {
        public AlbumId AlbumId { get; } = new(albumId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
