using Storage.Infrastructure.Models;

namespace Storage.WebAPI.Endpoint;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapStorageEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/add", (ImageFile file) => { });

        return endpoints;
    }
}
