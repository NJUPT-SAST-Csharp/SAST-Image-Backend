using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.ColumnServices.Queries.GetColumn
{
    internal sealed class GetColumnQueryHandler(IColumnQueryRepository repository)
        : IQueryRequestHandler<GetColumnQuery, Result<ColumnDetailedDto>>
    {
        private readonly IColumnQueryRepository _repository = repository;

        public async Task<Result<ColumnDetailedDto>> Handle(
            GetColumnQuery request,
            CancellationToken cancellationToken
        )
        {
            var column = await _repository.GetColumnAsync(request.Id);

            if (column is null)
            {
                return Result.Fail(Error.NotFound<ColumnDetailedDto>());
            }

            var dto = ColumnDetailedDto.MapFrom(column);

            return Result.Return(dto);
        }
    }
}
