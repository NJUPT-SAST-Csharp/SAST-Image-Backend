using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.UpdateAlbumInfo;

public sealed record UpdateAlbumInfoCommand(
    AlbumId AlbumId,
    AlbumTitle Title,
    AlbumDescription Description,
    CategoryId CategoryId,
    Accessibility Accessibility,
    Requester User
) : ICommand;
