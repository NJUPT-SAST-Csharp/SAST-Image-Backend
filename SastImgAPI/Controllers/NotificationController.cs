using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public NotificationController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves notifications for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Allows authenticated users to retrieve their notifications.
        /// </remarks>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "OK: Notifications retrieved successfully.",
            typeof(Notification[])
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: Notifications not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> GetNotifications(CancellationToken clt)
        {
            try
            {
                // Retrieve notifications for the authenticated user
                var notifications = await _dbContext.Users
                    .Include(user => user.Notifications)
                    .Where(user => user.Id == int.Parse(User.FindFirstValue("id")!))
                    .Select(user => user.Notifications)
                    .FirstAsync(clt);

                // Return the retrieved notifications
                return ResponseDispatcher.Data(notifications);
            }
            catch
            {
                // Return a 404 Not Found response if notifications are not found
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Notifications not found.")
                    .Build();
            }
        }

        /// <summary>
        /// Marks a specific notification as read.
        /// </summary>
        /// <remarks>
        /// Allows users to mark a specific notification as read by providing its unique ID.
        /// </remarks>
        /// <param name="id">The unique ID of the notification to mark as read.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPatch("{id:int}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: Notification marked as read successfully."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: Notification not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> ReadNotifications(int id, CancellationToken clt)
        {
            // Find the notification by its unique ID
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(
                x => x.Id == id,
                clt
            );

            // Check if the notification exists
            if (notification is null)
            {
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status404NotFound,
                        "Couldn't find the specific notification."
                    )
                    .Build();
            }

            // Mark the notification as read
            notification.IsRead = true;

            // Save the changes to the database
            await _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating successful marking as read
            return NoContent();
        }

        /// <summary>
        /// Deletes all notifications for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Allows authenticated users to delete all of their notifications.
        /// </remarks>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: Notifications deleted successfully."
        )]
        public async Task<IActionResult> DeleteAllNotifications(CancellationToken clt)
        {
            // Retrieve all notifications for the authenticated user
            var notifications = await _dbContext.Users
                .Where(user => user.Id == int.Parse(User.FindFirstValue("id")!))
                .Select(user => user.Notifications)
                .ToListAsync(clt);

            // Remove all retrieved notifications
            _dbContext.RemoveRange(notifications);

            // Save the changes to the database
            await _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating successful deletion
            return NoContent();
        }
    }
}
