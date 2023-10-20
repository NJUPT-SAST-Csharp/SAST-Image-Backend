using Shared.Primitives;
using Shared.Utilities;

namespace Album.Domain.Entities
{
    public sealed class Tag : AggregateRoot<long>
    {
        public Tag(string name)
            : base(SnowFlakeIdGenerator.NewId)
        {
            Name = name;
        }

        public string Name { get; private init; }
    }
}
