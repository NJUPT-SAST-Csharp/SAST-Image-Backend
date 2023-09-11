using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;

namespace SastImgAPI.Models.ResponseDtos
{
    public class AlbumResponseDto
    {
        public AlbumResponseDto(Album album)
        {
            Id = CodeAccessor.ToBase64String(album.Id);
            Name = album.Name;
            Description = album.Description;
            Cover = album.Cover;
            CreatedAt = album.CreatedAt;
            Accessibility = album.Accessibility;
            Author = new(album.Author);
        }

        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public Uri? Cover { get; init; }
        public DateTime CreatedAt { get; init; }
        public Accessibility Accessibility { get; init; }
        public AuthorDto Author { get; init; }

        public class AuthorDto
        {
            public AuthorDto(User user)
            {
                Id = CodeAccessor.ToBase64String(user.Id);
                Username = user.UserName!;
                Nickname = user.Nickname;
            }

            public string Id { get; init; }
            public string Username { get; init; }
            public string Nickname { get; init; }
        }
    }
}
