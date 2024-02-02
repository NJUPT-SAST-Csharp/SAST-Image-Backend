namespace SNS.WebAPI.Messages
{
    public readonly struct AlbumCreatedMessage
    {
        public readonly long AuthorId { get; init; }
        public readonly long AlbumId { get; init; }
    }
}
