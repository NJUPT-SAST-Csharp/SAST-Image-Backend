using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Domain.CategoryAggregate.Events;

namespace Square.Application.CategoryServices
{
    public sealed class CategoryModel
    {
        private CategoryModel() { }

        public CategoryId Id { get; private init; }

        public CategoryName Name { get; private set; }

        public static CategoryModel CreateNewCategory(CategoryCreatedEvent e)
        {
            return new() { Id = e.CategoryId, Name = e.Name };
        }
    }
}
