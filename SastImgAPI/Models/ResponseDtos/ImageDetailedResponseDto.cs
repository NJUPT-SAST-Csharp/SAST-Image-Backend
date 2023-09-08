using SastImgAPI.Models.DbSet;

namespace SastImgAPI.Models.ResponseDtos
{
    public class ImageDetailedResponseDto
    {
        public ImageDetailedResponseDto(
            int id,
            string name,
            string? description,
            int categoryId,
            Accessibility accessibility,
            bool isExifEnabled,
            int views,
            ICollection<int> likes,
            ICollection<string> tags,
            AuthorDto author,
            AlbumDto from
        )
        {
            Id = id;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Accessibility = accessibility;
            IsExifEnabled = isExifEnabled;
            Views = views;
            Likes = likes;
            Tags = tags;
            Author = author;
            From = from;
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public int CategoryId { get; init; }
        public Accessibility Accessibility { get; init; }
        public bool IsExifEnabled { get; init; }
        public int Views { get; init; }
        public ICollection<string> Tags { get; init; }
        public ICollection<int> Likes { get; init; }

        public AuthorDto Author { get; init; }
        public AlbumDto From { get; init; }

        public record AuthorDto(int Id, string Username, string Nickname);

        public record AlbumDto(int Id, string Name);
    }
}
