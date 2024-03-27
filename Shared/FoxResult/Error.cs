namespace FoxResult
{
    public record class Error
    {
        public string Message { get; internal init; } = string.Empty;
        public int Code { get; internal init; } = 0;

        public static readonly Error None = new();
    }
}
