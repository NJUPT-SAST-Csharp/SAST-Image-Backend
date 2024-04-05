using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.CategoryServices.Queries.GetCategories
{
    internal sealed class GetCategoriesQueryHandler(ICategoryQueryRepository repository)
        : IQueryRequestHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryQueryRepository _repository = repository;

        public async Task<Result<IEnumerable<CategoryDto>>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            var categories = await _repository.GetCategoriesAsync();

            var dtos = categories.Select(CategoryDto.MapFrom);

            return Result.Return(dtos);
        }
    }
}
