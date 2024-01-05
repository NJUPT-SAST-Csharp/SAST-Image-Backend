namespace Response.ResponseObjects
{
    public abstract class ResponseObject
    {
        public abstract string Type { get; }
        public abstract string Title { get; }
        public abstract int Status { get; }
    }
}
