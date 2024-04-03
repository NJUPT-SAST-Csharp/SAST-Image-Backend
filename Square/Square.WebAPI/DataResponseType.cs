namespace Square.WebAPI
{
    public sealed class DataResponseType<T>
    {
        private DataResponseType() { }

        public T Data { get; init; }
        public int Status { get; init; } = 200;
    }
}
