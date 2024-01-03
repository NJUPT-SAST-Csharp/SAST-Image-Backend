using Primitives.Entity;
using Shared.Utilities;

namespace SastImg.Domain.CategoryEntity
{
    public sealed class Category : EntityBase<long>
    {
        private Category(string name, string description)
            : base(SnowFlakeIdGenerator.NewId)
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
