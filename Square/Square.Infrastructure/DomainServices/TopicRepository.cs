using Microsoft.EntityFrameworkCore;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainServices
{
    internal sealed class TopicRepository(SquareDbContext context) : ITopicRepository
    {
        private readonly SquareDbContext _context = context;

        public void AddTopic(Topic topic)
        {
            _context.Topics.Add(topic);
        }

        public void DeleteTopic(Topic topic)
        {
            _context.Topics.Remove(topic);
        }

        public async Task<Topic?> GetTopicAsync(TopicId topicId)
        {
            return await _context.Topics.FirstOrDefaultAsync(t => t.Id == topicId);
        }
    }
}
