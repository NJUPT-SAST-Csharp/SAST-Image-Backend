using SastImg.Domain.TagEntity;

namespace SastImg.Application.TagServices
{
    public interface ITagQueryRepository
    {
        public Task<IEnumerable<TagDto>> GetAllTagsAsync(
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<TagDto>> GetTagsAsync(
            TagId[] ids,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<TagDto>> SearchTagsAsync(
            string name,
            CancellationToken cancellationToken = default
        );
    }
}
