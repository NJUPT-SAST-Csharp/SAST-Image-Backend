namespace Square.Domain.CategoryAggregate.CategoryEntity
{
    public readonly record struct CategoryName(string Value)
    {
        public override readonly string ToString()
        {
            return Value;
        }
    }
}
