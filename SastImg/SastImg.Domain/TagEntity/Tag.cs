using Primitives.Entity;
using Shared.Utilities;

namespace SastImg.Domain.TagEntity
{
    public sealed class Tag : EntityBase<TagId>
    {
        private Tag(string name)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _name = name;
        }

        private readonly string _name;

        public static Tag CreateNewTag(string name) => new(name);
    }
}
