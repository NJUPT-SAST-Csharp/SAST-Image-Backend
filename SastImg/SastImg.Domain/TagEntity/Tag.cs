using Primitives.Entity;
using Shared.Utilities;

namespace SastImg.Domain.TagEntity
{
    public sealed class Tag : EntityBase<long>
    {
        private Tag(string name)
            : base(SnowFlakeIdGenerator.NewId)
        {
            _name = name;
        }

        private readonly string _name;

        internal static Tag CreateNewTag(string name) => new(name);
    }
}
