using FoxResult;
using Primitives.Entity;
using Square.Domain.CategoryAggregate.Commands.CreateCategory;
using Square.Domain.CategoryAggregate.Events;

namespace Square.Domain.CategoryAggregate.CategoryEntity
{
    public sealed class Category : EntityBase<CategoryId>
    {
        private Category()
            : base(default) { }

        private Category(CategoryName name)
            : base(new(0))
        {
            _name = name;
        }

        #region Fields

        private CategoryName _name;

        #endregion

        #region Methods

        public static async Task<Result<Category>> CreateNewCategoryAsync(
            CreateCategoryCommand command,
            ICategoryUniquenessChecker checker,
            ICategoryRepository repository
        )
        {
            if (await checker.IsConflictAsync(command.Name))
            {
                return Result.Fail(Error.Conflict<Category>());
            }

            Category category = new(command.Name);

            category.AddDomainEvent(new CategoryCreatedEvent(category.Id, command.Name));

            repository.AddCategory(category);

            return Result.Return(category);
        }

        #endregion
    }
}
