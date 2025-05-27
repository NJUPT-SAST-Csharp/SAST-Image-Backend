using Exceptions.Exceptions;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.DomainRepositories;

internal sealed class TagRepository(SastImgDbContext context) : ITagRepository
{
    private readonly SastImgDbContext _context = context;

    public async Task<ImageTagId> AddTagAsync(
        ImageTag tag,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _context.Tags.AddAsync(tag, cancellationToken);
        return entity.Entity.Id;
    }

    public async Task<ImageTag> GetTagAsync(
        ImageTagId id,
        CancellationToken cancellationToken = default
    )
    {
        var tag = await _context.Tags.FindAsync([id], cancellationToken);
        if (tag is null)
        {
            throw new DbNotFoundException(nameof(ImageTag), id.Value.ToString());
        }

        return tag;
    }
}
