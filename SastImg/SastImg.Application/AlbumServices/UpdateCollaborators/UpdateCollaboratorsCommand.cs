using Identity;
using Mediator;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.UpdateCollaborators;

public sealed record class UpdateCollaboratorsCommand(
    AlbumId AlbumId,
    UserId[] Collaborators,
    Requester Requester
) : ICommand { }
