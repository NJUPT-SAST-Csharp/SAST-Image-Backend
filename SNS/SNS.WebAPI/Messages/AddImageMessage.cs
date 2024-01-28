using Messenger;

namespace SNS.WebAPI.Messages
{
    public readonly struct AddImageMessage : IMessage
    {
        public long ImageId { get; private init; }

        public long AuthorId { get; private init; }

        public long AlbumId { get; private init; }
    }
}
