using Shared.Primitives.Query;

namespace SastImg.Application.CategoryServices.GetAllCategory
{
    public sealed class GetAllCategoryQueryRequestHandler(ICategoryQueryRepository repository)
        : IQueryRequestHandler<GetAllCategoryQueryRequest, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryQueryRepository _repository = repository;

        public Task<IEnumerable<CategoryDto>> Handle(
            GetAllCategoryQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
