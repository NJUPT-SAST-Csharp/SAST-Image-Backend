using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Square.Application.TopicServices;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.ApplicationServices
{
    internal sealed class TopicQueryRepository(SquareQueryDbContext context) : ITopicQueryRepository
    {
        private readonly SquareQueryDbContext _context = context;

        public async Task AddTopicAsync(TopicModel topic)
        {
            await _context.AddAsync(topic);
        }

        public async Task DeleteTopicAsync(TopicId id)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);

            if (topic is null)
            {
                throw new DbNotFoundException(nameof(TopicModel), id.ToString());
            }

            _context.Remove(topic);
        }

        public Task<TopicModel?> GetTopicAsync(TopicId id)
        {
            return _context.Topics.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TopicModel>> GetTopicsAsync()
        {
            return await _context.Topics.ToListAsync();
        }
    }
}
