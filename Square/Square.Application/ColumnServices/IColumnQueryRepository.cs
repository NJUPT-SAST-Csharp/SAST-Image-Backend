using Square.Application.ColumnServices.Models;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.ColumnServices
{
    public interface IColumnQueryRepository
    {
        public Task<ColumnModel?> GetColumnAsync(ColumnId id);

        public Task<IEnumerable<ColumnModel>> GetColumnsAsync(TopicId topicId);

        public Task AddColumnAsync(ColumnModel column);

        public Task DeleteColumnAsync(ColumnId id);
    }
}
