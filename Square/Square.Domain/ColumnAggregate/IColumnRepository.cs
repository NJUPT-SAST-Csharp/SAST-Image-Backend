using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.ColumnAggregate
{
    public interface IColumnRepository
    {
        public Task<Column?> GetColumnAsync(TopicId topicId, UserId authorId);

        public Task<Column?> GetColumnAsync(ColumnId columnId);

        public void DeleteColumn(Column column);

        public void AddColumn(Column column);
    }
}
