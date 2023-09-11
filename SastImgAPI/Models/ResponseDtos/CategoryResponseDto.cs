using SastImgAPI.Models.DbSet;
using SastImgAPI.Services;

namespace SastImgAPI.Models.ResponseDtos
{
    public class CategoryResponseDto
    {
        public CategoryResponseDto(Category category)
        {
            Id = CodeAccessor.ToBase64String(category.Id);
            Name = category.Name;
            Description = category.Description;
        }

        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
