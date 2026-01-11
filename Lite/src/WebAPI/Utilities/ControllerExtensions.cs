using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Utilities;

public static class ControllerExtensions
{
    public static IActionResult DataOrNotFound(this ControllerBase controller, object? data)
    {
        return data is null ? controller.NotFound() : controller.Ok(data);
    }

    public static IActionResult ImageOrNotFound(this ControllerBase controller, Stream? image)
    {
        return image is null ? controller.NotFound() : controller.File(image, "image/*");
    }

    public static IActionResult AvatarOrNotFound(this ControllerBase controller, Stream? avatar)
    {
        return avatar is null ? controller.NotFound() : controller.File(avatar, "image/*");
    }

    public static IActionResult HeaderOrNotFound(this ControllerBase controller, Stream? header)
    {
        return header is null ? controller.NoContent() : controller.File(header, "image/*");
    }
}
