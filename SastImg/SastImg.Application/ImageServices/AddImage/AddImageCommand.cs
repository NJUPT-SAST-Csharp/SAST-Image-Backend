using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.TagEntity;

namespace SastImg.Application.ImageServices.AddImage
{
    public sealed class AddImageCommand(
        string title,
        string description,
        long[] tags,
        IFormFile file,
        long albumId,
        ClaimsPrincipal user
    ) : ICommandRequest<ImageInfoDto>, IDisposable
    {
        public Stream ImageFile { get; } = file.OpenReadStream();

        public string FileName { get; } = file.FileName;

        public AlbumId AlbumId { get; } = new(albumId);

        public string Title { get; } = title;

        public string Description { get; } = description;

        public TagId[] Tags { get; } = Array.ConvertAll(tags, tag => new TagId(tag));

        public RequesterInfo Requester { get; } = new(user);

        public void Dispose()
        {
            ImageFile.Dispose();
        }
    }
}
