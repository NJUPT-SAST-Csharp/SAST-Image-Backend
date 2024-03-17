namespace Square.Domain.TopicAggregate.TopicEntity
{
    public readonly record struct TopicId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
