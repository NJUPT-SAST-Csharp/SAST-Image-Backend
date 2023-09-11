using SastImgAPI.Models.DbSet;
using SastImgAPI.Services;
using Image = SastImgAPI.Models.DbSet.Image;

namespace SastImgAPI.Models.ResponseDtos
{
    public class ImageResponseDto
    {
        public ImageResponseDto(Image image)
        {
            Id = CodeAccessor.ToBase64String(image.Id);
            Url = image.Url;
            Title = image.Title;
            Description = image.Description;
            Category = CodeAccessor.ToBase64String(image.CategoryId);
            CreatedAt = image.CreatedAt;
            Author = image.Author.UserName!;
            Accessibility = image.Album.Accessibility;
            Tags = image.Tags.Select(tag => CodeAccessor.ToBase64String(tag.Id)).ToList();
        }

        public string Id { get; init; }
        public Uri Url { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Category { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Author { get; init; }
        public Accessibility Accessibility { get; init; }
        public ICollection<string> Tags { get; init; }
    }
}
