using Mediator;
using Primitives;
using SastImg.Domain.Categories;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.CategoryServices.CreateCategory;

public sealed class CreateCategoryCommandHandler(
    ICategoryRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCategoryCommand>
{
    public async ValueTask<Unit> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = Category.CreateNewCategory(request.Name, request.Description);
        await repository.AddCatergoryAsync(category, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
