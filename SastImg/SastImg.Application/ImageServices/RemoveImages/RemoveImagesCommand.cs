using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.RemoveImages
{
    public sealed class RemoveImagesCommand(
        long albumId,
        IEnumerable<long> imageIds,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public AlbumId AlbumId { get; } = new(albumId);

        public IEnumerable<ImageId> ImageIds { get; } = imageIds.Select(id => new ImageId(id));

        public RequesterInfo Requester { get; } = new(user);
    }
}
