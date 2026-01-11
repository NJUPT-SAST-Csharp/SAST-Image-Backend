using Application.Shared;
using Mediator;

namespace Application.CategoryServices.Queries;

public record class CategoryDto(long Id, string Name, string Description);

public record class CategoriesQuery() : IQuery<CategoryDto[]>;

internal sealed class CategoriesQueryHandler(
    IQueryRepository<CategoriesQuery, CategoryDto[]> repository
) : IQueryHandler<CategoriesQuery, CategoryDto[]>
{
    public async ValueTask<CategoryDto[]> Handle(
        CategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
