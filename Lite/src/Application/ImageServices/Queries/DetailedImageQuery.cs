using Application.Shared;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Application.ImageServices.Queries;

public sealed class DetailedImage
{
    public required long Id { get; init; }
    public required long AlbumId { get; init; }
    public required long UploaderId { get; init; }
    public required string Title { get; init; }
    public required DateTime UploadedAt { get; init; }
    public required string[] Tags { get; init; }
    public required int Likes { get; init; }
    public required RequesterInfo Requester { get; init; }

    public readonly record struct RequesterInfo(bool Liked);
}

public sealed record DetailedImageQuery(ImageId Image, Actor Actor) : IQuery<DetailedImage?>;

internal sealed class DetailedImageQueryHandler(
    IQueryRepository<DetailedImageQuery, DetailedImage?> repository
) : IQueryHandler<DetailedImageQuery, DetailedImage?>
{
    public async ValueTask<DetailedImage?> Handle(
        DetailedImageQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
