using Domain.CategoryAggregate.Events;

namespace Application.CategoryServices;

public sealed class CategoryModel
{
    private CategoryModel() { }

    internal CategoryModel(CategoryCreatedEvent e)
    {
        Id = e.Id.Value;
        Name = e.Name.Value;
        Description = e.Description.Value;
    }

    public long Id { get; init; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    internal void UpdateName(CategoryNameUpdatedEvent e)
    {
        Name = e.Name.Value;
    }

    internal void UpdateDescription(CategoryDescriptionUpdatedEvent e)
    {
        Description = e.Description.Value;
    }
}
