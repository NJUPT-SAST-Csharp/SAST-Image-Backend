using Microsoft.EntityFrameworkCore;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainServices
{
    internal sealed class TopicUniquenessChecker(SquareDbContext context) : ITopicUniquenessChecker
    {
        private readonly SquareDbContext _context = context;

        public Task<bool> IsConflictAsync(TopicTitle title)
        {
            return _context
                .Topics.AsNoTracking()
                .AnyAsync(t => EF.Property<TopicTitle>(t, "_title") == title);
        }
    }
}
