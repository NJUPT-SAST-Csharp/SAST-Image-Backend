namespace Common.Primitives
{
    public interface IDomainEvent
    {
        public DateTime RegisteredAt { get; }
    }
}
