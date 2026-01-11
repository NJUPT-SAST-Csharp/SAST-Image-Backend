using Domain.CategoryAggregate.Commands;
using Domain.CategoryAggregate.Events;
using Domain.Entity;
using Domain.Shared;

namespace Domain.CategoryAggregate.CategoryEntity;

public sealed class Category : EntityBase<CategoryId>
{
    private Category()
        : base(default) { }

    public Category(CreateCategoryCommand command)
        : base(CategoryId.GenerateNew())
    {
        if (command.Actor.IsAdmin == false)
            throw new NoPermissionException();

        _name = command.Name;
        AddDomainEvent(new CategoryCreatedEvent(Id, command.Name, command.Description));
    }

    private CategoryName _name;

    public void UpdateName(UpdateCategoryNameCommand command)
    {
        if (command.Actor.IsAdmin == false)
            throw new NoPermissionException();

        _name = command.Name;
        AddDomainEvent(new CategoryNameUpdatedEvent(Id, command.Name));
    }

    public void UpdateDescription(UpdateCategoryDescriptionCommand command)
    {
        if (command.Actor.IsAdmin == false)
            throw new NoPermissionException();
        AddDomainEvent(new CategoryDescriptionUpdatedEvent(Id, command.Description));
    }
}
