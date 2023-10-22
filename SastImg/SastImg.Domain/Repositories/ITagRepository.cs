using SastImg.Domain.Entities;

namespace SastImg.Domain.Repositories
{
    public interface ITagRepository
    {
        public Task DeleteTagByIdAsync(long id, CancellationToken cancellationToken = default);

        public Task DeleteTagByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        public Task<long> CreateTagAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        public Task<Tag> GetTagByIdAsync(long id, CancellationToken cancellationToken = default);

        public Task<Tag> GetTagByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );
    }
}
