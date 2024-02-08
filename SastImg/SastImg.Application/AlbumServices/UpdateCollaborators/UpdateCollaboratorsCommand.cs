using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.UpdateCollaborators
{
    public sealed class UpdateCollaboratorsCommand(
        long albumId,
        long[] collaborators,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public AlbumId AlbumId { get; } = new(albumId);

        public UserId[] Collaborators { get; } =
            Array.ConvertAll(collaborators, c => new UserId(c));

        public RequesterInfo Requester { get; } = new(user);
    }
}
