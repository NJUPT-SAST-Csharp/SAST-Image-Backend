using Shared.Primitives.Query;

namespace SastImg.Application.CategoryServices.GetAllCategory
{
    public sealed class GetAllCategoriesQueryHandler(ICategoryQueryRepository repository)
        : IQueryRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryQueryRepository _repository = repository;

        public Task<IEnumerable<CategoryDto>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
