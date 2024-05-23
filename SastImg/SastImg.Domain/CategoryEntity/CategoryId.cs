namespace SastImg.Domain.CategoryEntity
{
    public readonly record struct CategoryId(int Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
