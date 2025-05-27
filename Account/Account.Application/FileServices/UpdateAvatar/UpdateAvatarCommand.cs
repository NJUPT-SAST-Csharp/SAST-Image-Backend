using Identity;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.FileServices.UpdateAvatar;

public sealed record class UpdateAvatarCommand(IFormFile Avatar, Requester Requester) : ICommand;
