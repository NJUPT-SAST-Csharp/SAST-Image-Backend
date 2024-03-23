using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainRepositories
{
    internal sealed class TopicRepository(SquareDbContext context) : ITopicRepository
    {
        private readonly SquareDbContext _context = context;

        public async Task<TopicId> AddTopicAsync(Topic topic)
        {
            var topicEntity = await _context.Topics.AddAsync(topic);
            return topicEntity.Entity.Id;
        }
    }
}
