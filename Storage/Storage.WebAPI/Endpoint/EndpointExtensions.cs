using Mediator;
using Storage.Application.Commands;
using Storage.Infrastructure.Models;

namespace Storage.WebAPI.Endpoint;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapStorageEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/add",
            (ImageFile file, IMediator mediator) =>
                mediator.Send(new AddImageCommand(file, "default"))
        );

        return endpoints;
    }
}
