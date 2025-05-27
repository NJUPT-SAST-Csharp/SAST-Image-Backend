using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.RestoreAlbum;

public sealed record class RestoreAlbumCommand(AlbumId AlbumId, Requester Requester) : ICommand;
