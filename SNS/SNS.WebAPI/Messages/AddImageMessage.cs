using Messenger;

namespace SNS.WebAPI.Messages
{
    public readonly struct AddImageMessage : IMessage
    {
        public readonly long ImageId { get; private init; }

        public readonly long AuthorId { get; private init; }

        public readonly long AlbumId { get; private init; }
    }
}
