using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.RemoveAlbum;

public sealed record class RemoveAlbumCommand(AlbumId AlbumId, Requester Requester) : ICommand;
