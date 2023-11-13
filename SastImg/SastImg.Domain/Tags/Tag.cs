using Shared.DomainPrimitives;
using Shared.Utilities;

namespace SastImg.Domain.Tags
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
