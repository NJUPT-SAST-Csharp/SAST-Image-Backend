using FoxResult;
using Primitives.Command;
using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Domain.CategoryAggregate.Commands.CreateCategory
{
    public sealed class CreateCategoryCommand(string name) : ICommandRequest<Result>
    {
        public CategoryName Name { get; } = new(name);
    }
}
