namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    public readonly record struct AlbumId(long Value)
    {
        public override string ToString() => Value.ToString();
    }
}
