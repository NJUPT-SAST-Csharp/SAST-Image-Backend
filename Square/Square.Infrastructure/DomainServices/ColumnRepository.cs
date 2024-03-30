using Microsoft.EntityFrameworkCore;
using Square.Domain;
using Square.Domain.ColumnAggregate;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainServices
{
    internal sealed class ColumnRepository(SquareDbContext context) : IColumnRepository
    {
        private readonly SquareDbContext _context = context;

        public void AddColumn(Column column)
        {
            _context.Columns.Add(column);
        }

        public void DeleteColumn(Column column)
        {
            _context.Columns.Remove(column);
        }

        public async Task<Column?> GetColumnAsync(ColumnId id)
        {
            return await _context.Columns.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Column?> GetColumnAsync(TopicId topicId, UserId authorId)
        {
            return await _context.Columns.FirstOrDefaultAsync(
                c =>
                    EF.Property<TopicId>(c, "_topicId") == topicId
                    && EF.Property<UserId>(c, "_authorId") == authorId
            );
        }
    }
}
