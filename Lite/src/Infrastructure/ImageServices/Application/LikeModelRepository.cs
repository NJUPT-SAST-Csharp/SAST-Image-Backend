using Application.ImageServices;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Database;

namespace Infrastructure.ImageServices.Application;

internal sealed class LikeModelRepository(QueryDbContext context) : ILikeModelRepository
{
    public async Task AddAsync(LikeModel entity, CancellationToken cancellationToken = default)
    {
        await context.Likes.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(
        ImageId image,
        UserId user,
        CancellationToken cancellationToken = default
    )
    {
        var like = await GetOrDefaultAsync(image, user, cancellationToken);

        if (like is not null)
            context.Likes.Remove(like);
    }

    public async Task<LikeModel> GetAsync(
        ImageId image,
        UserId user,
        CancellationToken cancellationToken = default
    )
    {
        var like =
            await GetOrDefaultAsync(image, user, cancellationToken)
            ?? throw new EntityNotFoundException();

        return like;
    }

    private Task<LikeModel?> GetOrDefaultAsync(
        ImageId image,
        UserId user,
        CancellationToken cancellationToken = default
    )
    {
        return context.Likes.FindAsync([image.Value, user.Value], cancellationToken).AsTask();
    }
}
