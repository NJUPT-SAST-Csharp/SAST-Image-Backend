using Identity;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Proxy.Auth;

internal sealed class AuthTransformProvider : ITransformProvider
{
    private static readonly UserId AnonymousId = Requester.Anonymous.Id;

    public void Apply(TransformBuilderContext context)
    {
        context.AddRequestTransform(Transformer);
    }

    public void ValidateCluster(TransformClusterValidationContext context) { }

    public void ValidateRoute(TransformRouteValidationContext context) { }

    private static ValueTask Transformer(RequestTransformContext context)
    {
        var user = context.HttpContext.User;
        if (
            user.Identity?.IsAuthenticated is false
            || user.TryFetchClaim(nameof(UserId), out string? idValue) is false
            || user.TryFetchClaim(nameof(Role), out string? roleValue) is false
            || long.TryParse(idValue, out _) is false
            || Enum.TryParse<Role>(roleValue, out _) is false
        )
        {
            context.ProxyRequest.Headers.Cover(nameof(UserId), AnonymousId.Value.ToString());
            context.ProxyRequest.Headers.Cover(nameof(Role), nameof(Role.NONE));
            return ValueTask.CompletedTask;
        }

        context.ProxyRequest.Headers.Cover(nameof(UserId), idValue);
        context.ProxyRequest.Headers.Cover(nameof(Role), roleValue);
        return ValueTask.CompletedTask;
    }
}

file static class HeaderExtensions
{
    public static void Cover(
        this System.Net.Http.Headers.HttpRequestHeaders headers,
        string key,
        string value
    )
    {
        headers.Remove(key);
        headers.Add(key, value);
    }
}
