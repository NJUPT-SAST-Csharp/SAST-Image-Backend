namespace Square.Domain
{
    public readonly record struct UserId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
