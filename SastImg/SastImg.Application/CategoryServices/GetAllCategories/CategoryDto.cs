using System.Text.Json.Serialization;

namespace SastImg.Application.CategoryServices.GetAllCategory
{
    public sealed class CategoryDto
    {
        [JsonConstructor]
        private CategoryDto() { }

        public long CategoryId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
