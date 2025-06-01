using Identity;
using Mediator;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Commands;

public sealed record class RestoreAlbumCommand(AlbumId AlbumId, Requester Requester) : ICommand;
