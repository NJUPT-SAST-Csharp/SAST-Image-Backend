using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Exceptions;
using Domain.AlbumAggregate.Services;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Domain;

internal sealed class AlbumTitleUniquenessChecker(DomainDbContext context)
    : IAlbumTitleUniquenessChecker
{
    private readonly DomainDbContext _context = context;

    public async Task CheckAsync(AlbumTitle title, CancellationToken cancellationToken = default)
    {
        bool isDuplicated = await _context
            .Albums.FromSql($"SELECT 1 FROM domain.albums WHERE title ILIKE {title.Value}")
            .AsNoTracking()
            .AnyAsync(cancellationToken);

        if (isDuplicated)
        {
            throw new AlbumTitleDuplicateException(title);
        }
    }
}
