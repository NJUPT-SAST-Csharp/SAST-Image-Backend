using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.CategoryServices.Queries.GetCategories
{
    public sealed class GetCategoriesQuery : IQueryRequest<Result<IEnumerable<CategoryDto>>> { }
}
