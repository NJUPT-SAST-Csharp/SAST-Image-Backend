namespace Account.Domain.RoleEntity
{
    public readonly record struct RoleId(int Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
