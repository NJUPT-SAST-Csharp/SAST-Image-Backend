using Primitives;
using Primitives.Command;
using SastImg.Domain.Categories;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.CategoryServices.CreateCategory
{
    internal sealed class CreateCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Category.CreateNewCategory(request.Name, request.Description);
            await _repository.AddCatergoryAsync(category, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
