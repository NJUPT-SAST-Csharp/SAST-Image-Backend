using Shared.Primitives;

namespace SastImg.Domain.Entities
{
    public sealed class Category : AggregateRoot<int>
    {
        public Category(string name)
            : base(default)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public void UpdateCategoryInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
