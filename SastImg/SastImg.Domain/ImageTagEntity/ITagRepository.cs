namespace SastImg.Domain.AlbumTagEntity;

public interface ITagRepository
{
    public Task<ImageTag> GetTagAsync(ImageTagId id, CancellationToken cancellationToken = default);

    public Task<ImageTagId> AddTagAsync(
        ImageTag tag,
        CancellationToken cancellationToken = default
    );
}
