using System.Diagnostics;
using Domain.AlbumAggregate.Exceptions;
using Domain.CategoryAggregate.Exceptions;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserAggregate.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Exceptions;

public sealed class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not DomainException)
            return false;

        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        ProblemDetails response = exception switch
        {
            DomainModelInvalidException ex => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.ParamName is null
                    ? $"The value [{ex.InputValue}] is invalid."
                    : $"The value of [{ex.ParamName}]: [{ex.InputValue}] is invalid.",
                Title = "Validation failed.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            AlbumRemovedException => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Album's been removed.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            ImageRemovedException => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Image's been removed.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            LoginException => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Username or password incorrect.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            RegistryCodeException => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Registry code incorrect.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            RefreshTokenInvalidException => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Refresh token invalid or expired.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            AlbumTitleDuplicateException ex => new()
            {
                Status = StatusCodes.Status409Conflict,
                Title = $"The title [{ex.Title.Value}] has been occupied.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
            },

            UsernameDuplicateException ex => new()
            {
                Status = StatusCodes.Status409Conflict,
                Title = $"The username [{ex.Username.Value}] has been occupied.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
            },

            CategoryNotFoundException ex => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"Couldn't find the category with id [{ex.Category.Value}].",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            },

            CategoryNameDuplicateException ex => new()
            {
                Status = StatusCodes.Status409Conflict,
                Title = $"The name [{ex.Name.Value}] has been occupied.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
            },

            CollaboratorsNotFoundException => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"Couldn't find specific users.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            },

            ImageNotFoundException ex => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"Couldn't find image with id [{ex.ImageId.Value}] in this album.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            },

            ImageTagsNotFoundException => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"Couldn't find specific tags.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            },

            NoPermissionException => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"Couldn't find entity.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            },

            EntityNotFoundException => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"Couldn't find entity.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            },

            _ => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Unknown domain exception.",
                Detail = exception.Message,
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },
        };

        response.Extensions.Add("traceId", traceId);
        httpContext.Response.StatusCode = response.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
