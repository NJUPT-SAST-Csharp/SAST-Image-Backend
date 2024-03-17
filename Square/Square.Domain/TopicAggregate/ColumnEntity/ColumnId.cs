namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public readonly record struct ColumnId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
