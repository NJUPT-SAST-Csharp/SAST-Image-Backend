namespace SastImg.Application.CategoryServices.GetAllCategory
{
    public sealed class CategoryDto
    {
        private CategoryDto() { }

        public required long CategoryId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }
}
