namespace Square.Application.CategoryServices.Queries.GetCategories
{
    public sealed class CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public static CategoryDto MapFrom(CategoryModel model) =>
            new() { Id = model.Id.Value, Name = model.Name.Value };
    }
}
