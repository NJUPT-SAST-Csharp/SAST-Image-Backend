namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public readonly record struct TopicImageId(long Value)
    {
        public override string ToString() => Value.ToString();
    }
}
