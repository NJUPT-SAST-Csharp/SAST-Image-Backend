using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate
{
    public interface IColumnRepository
    {
        public Task<IColumn?> GetColumnAsync(ColumnId id);

        public Task DeleteColumnAsync(IColumn column);

        public Task AddColumnAsync(IColumn column);
    }
}
