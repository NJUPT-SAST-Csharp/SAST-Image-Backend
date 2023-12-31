using SastImg.Application.TagServices;

namespace SastImg.Infrastructure.Domain.TagEntity
{
    internal class TagQueryRepository : ITagQueryRepository
    {
        public Task<IEnumerable<TagDto>> GetAllTagsAsync(
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagDto>> GetTagsAsync(
            long[] ids,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagDto>> SearchTagsAsync(
            string name,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }
    }
}
