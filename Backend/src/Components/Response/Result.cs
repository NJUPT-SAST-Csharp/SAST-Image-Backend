using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;

namespace Response;

public sealed class Result(int statusCode) : IResult, IDictionary<string, string>
{
    public Result(int statusCode, string title)
        : this(statusCode) => Title = title;

    public string this[string key]
    {
        get => Extensions[key];
        set => Extensions[key] = value;
    }

    public int StatusCode { get; init; } = statusCode;
    public string Title
    {
        init => Extensions["title"] = value;
    }
    public string? Detail
    {
        init
        {
            if (value is not null)
                Extensions["detail"] = value;
        }
    }

    public IDictionary<string, string> Extensions { get; init; } = new Dictionary<string, string>();
    public ICollection<string> Keys => Extensions.Keys;
    public ICollection<string> Values => Extensions.Values;
    public int Count { get; }
    public bool IsReadOnly { get; }

    public void Add(string key, string value) => Extensions.Add(key, value);

    public void Add(KeyValuePair<string, string> item) => Extensions.Add(item.Key, item.Value);

    public void Clear() => Extensions.Clear();

    public bool Contains(KeyValuePair<string, string> item) => Extensions.Contains(item);

    public bool ContainsKey(string key) => Extensions.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) =>
        Extensions.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => Extensions.GetEnumerator();

    public bool Remove(string key) => Extensions.Remove(key);

    public bool Remove(KeyValuePair<string, string> item) => Extensions.Remove(item.Key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        return Extensions.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Extensions.GetEnumerator();
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        JsonObject json = new() { ["status"] = StatusCode };

        if (Extensions.Count > 0)
            foreach (var extension in Extensions)
                json[extension.Key] = extension.Value;

        httpContext.Response.StatusCode = StatusCode;
        httpContext.Response.ContentType =
            StatusCode < 300 ? "application/json" : "application/problem+json";
        return httpContext.Response.WriteAsync(
            json.ToJsonString(JsonSerializerOptions.Web),
            httpContext.RequestAborted
        );
    }
}
