using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.RemoveImage
{
    public sealed class RemoveImageCommand(long albumId, long imageId, ClaimsPrincipal user)
        : ICommandRequest
    {
        public AlbumId AlbumId { get; } = new(albumId);

        public ImageId ImageId { get; } = new(imageId);

        public RequesterInfo Requester { get; } = new(user);
    }
}
