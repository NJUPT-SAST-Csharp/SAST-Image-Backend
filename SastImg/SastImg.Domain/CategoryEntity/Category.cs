using Primitives.Entity;
using Utilities;

namespace SastImg.Domain.CategoryEntity
{
    public sealed class Category : EntityBase<CategoryId>
    {
        private Category(string name, string description)
            : base(default)
        {
            _name = name;
            _description = description;
        }

        public static Category CreateNewCategory(string name, string description) =>
            new(name, description);

        private string _name;

        private string _description;

        public void UpdateCategoryInfo(string name, string description)
        {
            _name = name;
            _description = description;
        }
    }
}
