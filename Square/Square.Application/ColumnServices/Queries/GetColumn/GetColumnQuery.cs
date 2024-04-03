using FoxResult;
using Shared.Primitives.Query;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Application.ColumnServices.Queries.GetColumn
{
    public sealed class GetColumnQuery(long id) : IQueryRequest<Result<ColumnDetailedDto>>
    {
        public ColumnId Id { get; } = new(id);
    }
}
