using Identity;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.FileServices.UpdateHeader;

public sealed record class UpdateHeaderCommand(IFormFile Header, Requester Requester) : ICommand;
