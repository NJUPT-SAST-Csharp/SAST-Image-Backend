using Shared.Primitives.Query;

namespace SastImg.Application.CategoryServices.GetAllCategory
{
    public sealed class GetAllCategoryQueryHandler(ICategoryQueryRepository repository)
        : IQueryRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryQueryRepository _repository = repository;

        public Task<IEnumerable<CategoryDto>> Handle(
            GetAllCategoryQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
