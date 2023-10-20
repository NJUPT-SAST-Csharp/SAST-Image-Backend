using Shared.Primitives;

namespace Album.Domain.Entities
{
    public sealed class Category : AggregateRoot<int>
    {
        public Category(string name)
        {
            Name = name;
        }

        public string Name { get; private init; }

        public string Description { get; private set; } = string.Empty;

        public void UpdateDescription(string description) => Description = description;
    }
}
