using System.Security.Claims;
using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.GetImageFile;

public sealed class GetImageFileQuery(long imageId, bool isThumbnail, ClaimsPrincipal user)
    : IQuery<Stream?>
{
    public Requester Requester { get; } = new(user);
    public ImageId ImageId { get; } = new() { Value = imageId };
    public bool IsThumbnail { get; } = isThumbnail;
}
