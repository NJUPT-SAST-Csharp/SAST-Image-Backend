using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using Image = SastImgAPI.Models.DbSet.Image;

namespace SastImgAPI.Models.ResponseDtos
{
    public class AlbumDetailedResponseDto
    {
        public AlbumDetailedResponseDto(Album album)
        {
            Id = CodeAccessor.ToBase64String(album.Id);
            Name = album.Name;
            Description = album.Description;
            Cover = album.Cover;
            CreatedAt = album.CreatedAt;
            Accessibility = album.Accessibility;
            Author = new(album.Author);
            Images = album.Images.Select(image => new ImageDto(image)).ToList();
        }

        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public Uri? Cover { get; init; }
        public DateTime CreatedAt { get; init; }
        public Accessibility Accessibility { get; init; }
        public AuthorDto Author { get; init; }
        public ICollection<ImageDto> Images { get; init; }

        public class AuthorDto
        {
            public AuthorDto(User user)
            {
                Id = user.Id;
                Username = user.UserName!;
                Nickname = user.Nickname;
            }

            public long Id { get; init; }
            public string Username { get; init; }
            public string Nickname { get; init; }
        }

        public class ImageDto
        {
            public ImageDto(Image image)
            {
                Id = CodeAccessor.ToBase64String(image.Id);
                Title = image.Title;
                IsExifEnabled = image.IsExifEnabled;
            }

            public string Id { get; init; }
            public string Title { get; init; }
            public bool IsExifEnabled { get; init; }
        }
    }
}
