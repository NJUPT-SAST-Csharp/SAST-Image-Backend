using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain
{
    internal sealed class Tag : Entity<long>
    {
        private Tag(string name)
            : base(SnowFlakeIdGenerator.NewId)
        {
            _name = name;
        }

        private readonly string _name;

        internal string Name => _name;

        internal static Tag CreateNewTag(string name) => new(name);
    }
}
