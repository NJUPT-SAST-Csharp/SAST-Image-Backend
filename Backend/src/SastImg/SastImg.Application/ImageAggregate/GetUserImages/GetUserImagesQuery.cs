using Identity;
using Mediator;

namespace SastImg.Application.ImageServices.GetUserImages;

public sealed record class GetUserImagesQuery(UserId UserId, int Page, Requester Requester)
    : IQuery<IEnumerable<UserImageDto>>;
