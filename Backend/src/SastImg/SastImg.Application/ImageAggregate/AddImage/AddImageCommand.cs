using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.AlbumTagEntity;

namespace SastImg.Application.ImageServices.AddImage;

public sealed record class AddImageCommand(
    ImageTitle Title,
    ImageDescription Description,
    ImageTagId[] Tags,
    AlbumId AlbumId,
    Requester Requester
) : ICommand<ImageInfoDto>;
