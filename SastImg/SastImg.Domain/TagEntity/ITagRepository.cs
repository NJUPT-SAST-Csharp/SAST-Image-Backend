namespace SastImg.Domain.TagEntity
{
    public interface ITagRepository
    {
        public Task<Tag> GetTagAsync(TagId id, CancellationToken cancellationToken = default);

        public Task<TagId> AddTagAsync(Tag tag, CancellationToken cancellationToken = default);
    }
}
