using SastImgAPI.Models.DbSet;

namespace SastImgAPI.Models.ResponseDtos
{
    public class AlbumResponseDto
    {
        public AlbumResponseDto(
            int id,
            string name,
            string description,
            DateTime createdAt,
            Accessibility accessibility,
            AuthorDto author
        )
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            Accessibility = accessibility;
            Author = author;
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime CreatedAt { get; init; }
        public Accessibility Accessibility { get; init; }
        public AuthorDto Author { get; init; }

        public record AuthorDto(int Id, string Username, string Nickname);
    }
}
