using Mediator;
using Storage.Application.Commands;
using Storage.Application.Model;
using Storage.Application.Queries;
using Storage.Infrastructure.Models;

namespace Storage.WebAPI.Endpoint;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapStorageEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/add",
            async (HttpContext context, IMediator mediator) =>
            {
                if (context.TryGetImageFile(out var image) is false)
                    return Results.BadRequest("Invalid image file.");

                var result = await mediator.Send(new AddImageCommand(image, "default"));
                return result.Success
                    ? Results.Ok<Dictionary<string, object?>>(
                        new() { ["token"] = result.Token.Value }
                    )
                    : Results.InternalServerError(
                        "Upload failed but we just dunno what happened ;)"
                    );
            }
        );

        endpoints.MapGet("/formats", () => Enum.GetNames<ImageFileFormat>());
        endpoints.MapGet(
            "/image/{token:maxlength(128)}",
            async (string token, HttpResponse response, IMediator mediator) =>
            {
                bool file = await mediator.Send(new ImageFileQuery(token, response.BodyWriter));
                if (file is false)
                {
                    return Results.NotFound();
                }

                return Results.Ok();
            }
        );

        return endpoints;
    }
}
