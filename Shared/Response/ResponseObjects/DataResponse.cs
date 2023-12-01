using System.Diagnostics.CodeAnalysis;

namespace Response.ReponseObjects
{
    public sealed class DataResponse<T>(T data)
        where T : notnull
    {
        public T Data { get; } = data;
    }

    public sealed class DataResponse([DisallowNull] object obj)
    {
        public object Data { get; } = obj;
    }
}
