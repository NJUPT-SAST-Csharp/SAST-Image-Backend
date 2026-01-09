using System.Text.Json.Serialization;

namespace SastImg.Application.CategoryServices.GetAllCategory;

public sealed class CategoryDto
{
    [JsonConstructor]
    private CategoryDto() { }

    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
