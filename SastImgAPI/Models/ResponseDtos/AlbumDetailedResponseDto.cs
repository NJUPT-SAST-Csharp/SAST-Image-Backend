using Microsoft.AspNetCore.Http.HttpResults;
using SastImgAPI.Models.DbSet;

namespace SastImgAPI.Models.ResponseDtos
{
    public class AlbumDetailedResponseDto
    {
        public AlbumDetailedResponseDto(
            int id,
            string name,
            string description,
            DateTime createdAt,
            Accessibility accessibility,
            AuthorDto author,
            ICollection<ImageDto> images
        )
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            Accessibility = accessibility;
            Author = author;
            Images = images;
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime CreatedAt { get; init; }
        public Accessibility Accessibility { get; init; }
        public AuthorDto Author { get; init; }
        public ICollection<ImageDto> Images { get; init; }

        public record AuthorDto(int Id, string Username, string Nickname);

        public record ImageDto(int Id, string Title, bool IsExifEnabled);
    }
}
