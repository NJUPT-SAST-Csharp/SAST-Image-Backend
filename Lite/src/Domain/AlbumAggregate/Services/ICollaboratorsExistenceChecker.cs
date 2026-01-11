using Domain.AlbumAggregate.AlbumEntity;

namespace Domain.AlbumAggregate.Services;

public interface ICollaboratorsExistenceChecker
{
    public Task CheckAsync(
        Collaborators collaborators,
        CancellationToken cancellationToken = default
    );
}
