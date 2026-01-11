using Application.ImageServices;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageModelRepository(QueryDbContext context) : IImageModelRepository
{
    public async Task AddAsync(ImageModel entity, CancellationToken cancellationToken = default)
    {
        await context.Images.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        var image = await GetOrDefaultAsync(id, cancellationToken);
        if (image is not null)
            context.Images.Remove(image);
    }

    public async Task<ImageModel> GetAsync(
        ImageId id,
        CancellationToken cancellationToken = default
    )
    {
        var image =
            await GetOrDefaultAsync(id, cancellationToken) ?? throw new EntityNotFoundException();

        return image;
    }

    private Task<ImageModel?> GetOrDefaultAsync(
        ImageId id,
        CancellationToken cancellationToken = default
    )
    {
        return context
            .Images.Include(image => image.Likes)
            .FirstOrDefaultAsync(i => i.Id == id.Value, cancellationToken);
    }
}
