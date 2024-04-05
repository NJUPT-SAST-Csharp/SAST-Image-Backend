using FoxResult;
using Primitives.Command;
using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Domain.CategoryAggregate.Commands.CreateCategory
{
    internal sealed class CreateCategoryCommandHandler(
        ICategoryRepository repository,
        ICategoryUniquenessChecker checker
    ) : ICommandRequestHandler<CreateCategoryCommand, Result>
    {
        private readonly ICategoryRepository _repository = repository;
        private readonly ICategoryUniquenessChecker _checker = checker;

        public async Task<Result> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await Category
                .CreateNewCategoryAsync(request, _checker, _repository)
                .WaitAsync(cancellationToken);

            return result;
        }
    }
}
