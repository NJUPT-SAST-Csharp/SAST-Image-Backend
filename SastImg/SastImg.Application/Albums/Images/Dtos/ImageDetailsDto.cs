namespace SastImg.Application.Albums.Images.Dtos
{
    public record ImageDetailsDto(
        long ImageId,
        string Title,
        Uri Uri,
        string Description,
        bool IsHidden,
        int CategoryId,
        IEnumerable<long> Tags
    );
}
