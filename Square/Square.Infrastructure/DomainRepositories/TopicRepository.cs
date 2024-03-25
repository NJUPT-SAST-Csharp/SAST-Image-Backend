using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainRepositories
{
    internal sealed class TopicRepository(SquareDbContext context) : ITopicRepository
    {
        private readonly SquareDbContext _context = context;

        public async Task<TopicId> AddTopicAsync(
            Topic topic,
            CancellationToken cancellationToken = default
        )
        {
            var topicEntity = await _context.Topics.AddAsync(topic);
            return topicEntity.Entity.Id;
        }

        public async Task DeleteTopicAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default
        )
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(
                t => t.Id == topicId,
                cancellationToken
            );

            if (topic is null)
            {
                throw new DbNotFoundException(nameof(Topic), topicId.ToString());
            }

            _context.Topics.Remove(topic);
        }

        public async Task<Topic> GetTopicAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default
        )
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(
                t => t.Id == topicId,
                cancellationToken
            );

            if (topic is null)
            {
                throw new DbNotFoundException(nameof(Topic), topicId.ToString());
            }

            return topic;
        }
    }
}
