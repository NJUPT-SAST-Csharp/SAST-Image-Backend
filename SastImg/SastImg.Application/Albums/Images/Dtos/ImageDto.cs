namespace SastImg.Application.Albums.Images.Dtos
{
    public record ImageDto(long ImageId, string Title, Uri Uri, bool IsHidden);
}
