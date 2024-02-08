using Primitives.Command;

namespace SastImg.Application.TagServices.CreateTag
{
    public sealed class CreateTagCommand(string name) : ICommandRequest<TagDto>
    {
        public string Name { get; } = name;
    }
}
