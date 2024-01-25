using DotNetCore.CAP;

namespace Messenger
{
    public sealed class SubscribeMessageAttribute(string name, bool isPartial = false)
        : CapSubscribeAttribute(name, isPartial)
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
