using Microsoft.Extensions.Options;
using SastImgAPI.Models.Identity;
using SastImgAPI.Options;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using System.Reflection.Metadata.Ecma335;

namespace SastImgAPI.Services
{
    public class ImageAccessor
    {
        private readonly FileStorageOptions _options;

        public ImageAccessor(IOptionsSnapshot<FileStorageOptions> options)
        {
            _options = options.Value;
            var config = Configuration.Default;
            config.ReadOrigin = ReadOrigin.Begin;
        }

        public async Task<string> UploadImageAsync(IFormFile file, CancellationToken clt = default)
        {
            var fileName = string.Concat(
                Guid.NewGuid().ToString().AsSpan()[..8],
                Path.GetExtension(file.FileName)
            );

            string path = Path.Combine(
                _options.BaseUrl,
                "image",
                DateTime.Now.Year.ToString(),
                DateTime.Now.Month.ToString("00"),
                DateTime.Now.Day.ToString("00"),
                Path.GetFileNameWithoutExtension(fileName)
            );
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var url = Path.Combine(path, fileName);
            using (var stream = new FileStream(url, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream, clt);
                await CreateSizeOptionAsync(stream);
            }
            return url;
        }

        public async Task<string> UploadProfileAvatarAsync(
            IFormFile file,
            long userId,
            CancellationToken clt = default
        )
        {
            var filename = string.Concat(userId, ".png");
            string path = Path.Combine(_options.BaseUrl, "user", "avatar");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var url = Path.Combine(path, filename);
            using (var stream = new FileStream(url, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream, clt);
            }
            return url!;
        }

        public async Task<string> UploadProfileHeaderAsync(
            IFormFile file,
            long userId,
            CancellationToken clt = default
        )
        {
            var filename = string.Concat(userId, ".png");
            string path = Path.Combine(_options.BaseUrl, "user", "header");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var url = Path.Combine(path, filename);
            using (var stream = new FileStream(url, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream, clt);
            }
            return url;
        }

        public async Task<string> UploadAlbumCoverAsync(
            IFormFile file,
            long albumId,
            CancellationToken clt = default
        )
        {
            var filename = string.Concat(albumId, ".png");
            string path = Path.Combine(_options.BaseUrl, "album", "cover");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var url = Path.Combine(path, filename);
            using (var stream = new FileStream(url, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream, clt);
            }
            return url;
        }

        public string GetImageExtensionName(string fileName)
        {
            var contentTypDict = new Dictionary<string, string>
            {
                { "jpg", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "jpe", "image/jpeg" },
                { "png", "image/png" },
                { "gif", "image/gif" },
                { "ico", "image/x-ico" },
                { "tiff", "image/tiff" },
                { "tga", "image/x-tga" },
                { "wbmp", "image/vnd.wap.wbmp" },
                { "pbm", "image/x-portable-bitmap" },
            };
            var contentTypeStr = "image/png";
            var imgType = Path.GetExtension(fileName);
            if (contentTypDict.ContainsKey(imgType))
            {
                contentTypeStr = contentTypDict[imgType];
            }
            return contentTypeStr;
        }

        public void DeleteImage(string path)
        {
            Directory.Delete(Path.GetDirectoryName(path)!, true);
        }

        private static async Task ResizeImageAsync(FileStream imageStream, ImageSize size)
        {
            var ratio = size switch
            {
                ImageSize.Original => 1,
                ImageSize.Large => 0.8,
                ImageSize.Medium => 0.6,
                ImageSize.Small => 0.3,
                _
                    => throw new ArgumentOutOfRangeException(
                        nameof(size),
                        "Not in the 'ImageSize' enum."
                    )
            };
            if (ratio >= 1 || ratio <= 0)
                return;
            using var image = await Image.LoadAsync(imageStream);

            int width = Convert.ToInt32(image.Width * ratio);
            int height = Convert.ToInt32(image.Height * ratio);
            image.Mutate(
                x =>
                    x.Resize(
                        new ResizeOptions { Size = new Size(width, height), Mode = ResizeMode.Max }
                    )
            );
            var url = Path.Combine(
                Path.GetDirectoryName(imageStream.Name)!,
                Path.GetFileNameWithoutExtension(imageStream.Name)
                    + '_'
                    + size
                    + Path.GetExtension(imageStream.Name)
            );
            await image.SaveAsync(url);
        }

        private static async Task CreateSizeOptionAsync(FileStream origImageStream)
        {
            await ResizeImageAsync(origImageStream, ImageSize.Small);
            await ResizeImageAsync(origImageStream, ImageSize.Medium);
            await ResizeImageAsync(origImageStream, ImageSize.Large);
        }

        public enum ImageSize
        {
            Original,
            Large,
            Medium,
            Small,
        }
    }
}
