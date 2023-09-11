using SastImgAPI.Models.DbSet;
using SastImgAPI.Services;

namespace SastImgAPI.Models.ResponseDtos
{
    public class TagResponseDto
    {
        public TagResponseDto(Tag tag)
        {
            Id = CodeAccessor.ToBase64String(tag.Id);
            Name = tag.Name;
        }

        public string Id { get; init; }
        public string Name { get; init; }
    }
}
