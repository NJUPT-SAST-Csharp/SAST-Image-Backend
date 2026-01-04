using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumAggregate.CreateAlbum;

public sealed record class CreateAlbumCommand(
    AlbumTitle Title,
    AlbumDescription Description,
    CategoryId CategoryId,
    Accessibility Accessibility,
    Requester Requester
) : ICommand<CreateAlbumDto>;
