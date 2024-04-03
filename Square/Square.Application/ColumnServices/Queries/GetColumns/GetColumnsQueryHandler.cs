using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.ColumnServices.Queries.GetColumns
{
    internal sealed class GetColumnsQueryHandler(IColumnQueryRepository repository)
        : IQueryRequestHandler<GetColumnsQuery, Result<IEnumerable<ColumnDto>>>
    {
        private readonly IColumnQueryRepository _repository = repository;

        public async Task<Result<IEnumerable<ColumnDto>>> Handle(
            GetColumnsQuery request,
            CancellationToken cancellationToken
        )
        {
            var columns = await _repository
                .GetColumnsAsync(request.TopicId)
                .WaitAsync(cancellationToken);

            var dtos = columns.Select(ColumnDto.MapFrom);

            return Result.Return(dtos);
        }
    }
}
