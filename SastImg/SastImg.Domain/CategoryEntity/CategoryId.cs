namespace SastImg.Domain.CategoryEntity
{
    public readonly record struct CategoryId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
