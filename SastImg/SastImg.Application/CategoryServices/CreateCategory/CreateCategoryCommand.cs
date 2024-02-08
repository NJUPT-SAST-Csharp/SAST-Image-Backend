using Primitives.Command;

namespace SastImg.Application.CategoryServices.CreateCategory
{
    public sealed class CreateCategoryCommand(string name, string description) : ICommandRequest
    {
        public string Name { get; } = name;
        public string Description { get; } = description;
    }
}
