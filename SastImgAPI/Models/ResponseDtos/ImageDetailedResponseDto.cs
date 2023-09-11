using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using Image = SastImgAPI.Models.DbSet.Image;

namespace SastImgAPI.Models.ResponseDtos
{
    public class ImageDetailedResponseDto
    {
        public ImageDetailedResponseDto(Image image)
        {
            Id = CodeAccessor.ToBase64String(image.Id);
            Title = image.Title;
            Description = image.Description;
            Category = CodeAccessor.ToBase64String(image.CategoryId);
            Accessibility = image.Album.Accessibility;
            IsExifEnabled = image.IsExifEnabled;
            Views = image.Views;
            Likes = image.LikedBy.Select(CodeAccessor.ToBase64String).ToList();
            Tags = image.Tags.Select(tag => CodeAccessor.ToBase64String(tag.Id)).ToList();
            Author = new(image.Author);
            Album = new(image.Album);
        }

        public string Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Category { get; init; }
        public Accessibility Accessibility { get; init; }
        public bool IsExifEnabled { get; init; }
        public int Views { get; init; }
        public ICollection<string> Tags { get; init; }
        public ICollection<string> Likes { get; init; }
        public AuthorDto Author { get; init; }
        public AlbumDto Album { get; init; }

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

        public class AlbumDto
        {
            public AlbumDto(Album album)
            {
                Id = CodeAccessor.ToBase64String(album.Id);
                Name = album.Name;
            }

            public string Id { get; init; }
            public string Name { get; init; }
        }
    }
}
