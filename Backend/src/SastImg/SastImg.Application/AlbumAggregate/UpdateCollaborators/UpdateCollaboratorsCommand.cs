using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumAggregate.UpdateCollaborators;

public sealed record class UpdateCollaboratorsCommand(
    AlbumId AlbumId,
    UserId[] Collaborators,
    Requester Requester
) : ICommand { }
