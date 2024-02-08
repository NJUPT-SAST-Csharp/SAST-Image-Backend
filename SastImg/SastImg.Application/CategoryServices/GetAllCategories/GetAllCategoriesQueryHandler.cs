using SastImg.Application.SeedWorks;
using Shared.Primitives.Query;

namespace SastImg.Application.CategoryServices.GetAllCategory
{
    public sealed class GetAllCategoriesQueryHandler(
        ICategoryQueryRepository repository,
        ICache<IEnumerable<CategoryDto>> cache
    ) : IQueryRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryQueryRepository _repository = repository;
        private readonly ICache<IEnumerable<CategoryDto>> _cache = cache;

        public Task<IEnumerable<CategoryDto>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            return _cache.GetCachingAsync(string.Empty, cancellationToken)!;
        }
    }
}
