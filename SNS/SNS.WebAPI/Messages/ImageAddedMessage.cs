namespace SNS.WebAPI.Messages
{
    public readonly struct ImageAddedMessage
    {
        public readonly long ImageId { get; init; }
        public readonly long AuthorId { get; init; }
        public readonly long AlbumId { get; init; }
    }
}
