using Identity;
using Mediator;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Commands;

public sealed record class RemoveAlbumCommand(AlbumId AlbumId, Requester Requester) : ICommand;
