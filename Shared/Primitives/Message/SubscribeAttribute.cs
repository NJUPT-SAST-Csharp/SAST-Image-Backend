using DotNetCore.CAP;

namespace Primitives.Message
{
    public sealed class SubscribeAttribute(string name, bool isPartial = false)
        : CapSubscribeAttribute(name, isPartial)
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
