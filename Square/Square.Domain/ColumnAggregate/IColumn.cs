using FoxResult;
using Square.Domain.ColumnAggregate.ColumnEntity.Commands;

namespace Square.Domain.ColumnAggregate
{
    public interface IColumn
    {
        public Result Like(LikeColumnCommand command);
        public Result Unlike(UnlikeColumnCommand command);
    }
}
