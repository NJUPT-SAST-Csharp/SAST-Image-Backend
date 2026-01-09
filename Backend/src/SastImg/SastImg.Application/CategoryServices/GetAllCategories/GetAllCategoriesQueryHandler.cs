using Mediator;

namespace SastImg.Application.CategoryServices.GetAllCategory;

public sealed class GetAllCategoriesQueryHandler(ICategoryQueryRepository repository)
    : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async ValueTask<IEnumerable<CategoryDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetAllCategoriesAsync(cancellationToken);
    }
}
