namespace SastImg.Application.ImageServices.SearchImages;

public sealed class SearchedImageDto
{
    private SearchedImageDto() { }

    public long ImageId { get; init; }
    public long AlbumId { get; init; }
    public required string Title { get; init; }
}
