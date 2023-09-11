namespace SastImgAPI.Models.ResponseDtos
{
    using SastImgAPI.Services;
    using Image = DbSet.Image;

    public class ImageCreatedResponseDto
    {
        public ImageCreatedResponseDto(Image image)
        {
            Id = CodeAccessor.ToBase64String(image.Id);
        }

        public string Id { get; init; }
    }
}
