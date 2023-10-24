namespace Common.Primitives
{
    public interface IDomainEvent
    {
        public DateTime RegisteredTime { get; }
    }
}
