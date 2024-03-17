namespace Square.Domain.TopicAggregate
{
    public readonly record struct UserId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
