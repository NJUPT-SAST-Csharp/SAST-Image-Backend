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
        SquareDbContext context
    ) : IColumnQueryRepository
    {
        private readonly SquareDbContext _context = context;
        private readonly IColumnImageStorage _storage = storage;

        public async Task AddColumnAsync(ColumnModel column)
        {
            await _context.ColumnModels.AddAsync(column);
        }

        public async Task DeleteColumnAsync(ColumnId id)
        {
            var column = await _context.ColumnModels.FirstOrDefaultAsync(c => c.Id == id);

            if (column is null)
            {
                throw new DbNotFoundException(nameof(ColumnModel), id.ToString());
            }

            await _storage.DeleteImagesAsync(column.Images);
            _context.ColumnModels.Remove(column);
        }

        public Task<ColumnModel?> GetColumnAsync(ColumnId id)
        {
            return _context.ColumnModels.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ColumnModel>> GetColumnsAsync(TopicId topicId)
        {
            var columns = await _context
                .ColumnModels.Where(c => c.TopicId == topicId)
                .ToListAsync();

            return columns;
        }
    }
}
