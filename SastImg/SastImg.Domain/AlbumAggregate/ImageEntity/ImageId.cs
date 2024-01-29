namespace SastImg.Domain.AlbumAggregate.ImageEntity
{
    public readonly record struct ImageId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
