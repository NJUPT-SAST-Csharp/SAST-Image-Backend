using Primitives.Entity;

namespace SastImg.Domain.CategoryEntity;

public sealed class Category : EntityBase<CategoryId>
{
    private Category(CategoryName name, CategoryDescription description)
        : base(default)
    {
        _name = name;
        _description = description;
    }

    public static Category CreateNewCategory(CategoryName name, CategoryDescription description)
    {
        var category = new Category(name, description);

        return category;
    }

    private CategoryName _name;

    private CategoryDescription _description;

    public void UpdateCategoryInfo(CategoryName name, CategoryDescription description)
    {
        _name = name;
        _description = description;
    }
}
