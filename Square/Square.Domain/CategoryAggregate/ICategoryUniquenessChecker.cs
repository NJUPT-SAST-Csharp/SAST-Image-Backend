using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Domain.CategoryAggregate
{
    public interface ICategoryUniquenessChecker
    {
        public Task<bool> IsConflictAsync(CategoryName title);
    }
}
