using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Square.Application.ColumnServices;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.ApplicationServices
{
    internal sealed class ColumnQueryRepository(
        IColumnImageStorage storage,
        SquareQueryDbContext context
    ) : IColumnQueryRepository
    {
        private readonly SquareQueryDbContext _context = context;
        private readonly IColumnImageStorage _storage = storage;

        public async Task AddColumnAsync(ColumnModel column)
        {
            await _context.Columns.AddAsync(column);
        }

        public async Task DeleteColumnAsync(ColumnId id)
        {
            var column = await _context.Columns.FirstOrDefaultAsync(c => c.Id == id);

            if (column is null)
            {
                throw new DbNotFoundException(nameof(ColumnModel), id.ToString());
            }

            await _storage.DeleteImagesAsync(column.Images);
            _context.Columns.Remove(column);
        }

        public Task<ColumnModel?> GetColumnAsync(ColumnId id)
        {
            return _context.Columns.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ColumnModel>> GetColumnsAsync(TopicId topicId)
        {
            var columns = await _context.Columns.Where(c => c.TopicId == topicId).ToListAsync();

            return columns;
        }
    }
}
