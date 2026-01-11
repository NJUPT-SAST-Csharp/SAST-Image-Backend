using System.Diagnostics.CodeAnalysis;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Extensions;

namespace Domain.AlbumAggregate.Exceptions;

public sealed class CollaboratorsNotFoundException(Collaborators collaborators) : DomainException
{
    public Collaborators Collaborators { get; } = collaborators;

    [DoesNotReturn]
    public static void Throw(Collaborators collaborators)
    {
        throw new CollaboratorsNotFoundException(collaborators);
    }
}
