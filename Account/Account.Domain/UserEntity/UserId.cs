namespace Account.Domain.UserEntity
{
    public readonly record struct UserId(long Value)
    {
        public override string ToString() => Value.ToString();
    }
}
