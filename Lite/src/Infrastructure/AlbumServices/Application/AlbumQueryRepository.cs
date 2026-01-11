using Application.AlbumServices.Queries;
using Application.Shared;
using Domain.AlbumAggregate.AlbumEntity;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Application;

internal sealed class AlbumQueryRepository(QueryDbContext context)
    : IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>,
        IQueryRepository<AlbumsQuery, AlbumDto[]>,
        IQueryRepository<RemovedAlbumsQuery, RemovedAlbumDto[]>
{
    private readonly QueryDbContext _context = context;

    const int PageSize = 60;

    public Task<DetailedAlbum?> GetOrDefaultAsync(
        DetailedAlbumQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Albums.AsNoTracking()
            .Where(a => a.Id == query.Id.Value)
            .Where(a => a.Status == AlbumStatusValue.Available)
            .WhereIsAccessible(query.Actor)
            .Select(a => new DetailedAlbum
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Author = a.AuthorId,
                Category = a.CategoryId,
                Tags = a.Tags,
                UpdatedAt = a.UpdatedAt,
                CreatedAt = a.CreatedAt,
                AccessLevel = a.AccessLevel,
                SubscribeCount = a.Subscribes.Count,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<AlbumDto[]> GetOrDefaultAsync(
        AlbumsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Albums.AsNoTracking()
            .Where(a => a.Status == AlbumStatusValue.Available)
            .Where(a => query.CategoryId == null || a.CategoryId == query.CategoryId)
            .Where(a => query.AuthorId == null || a.AuthorId == query.AuthorId)
            .Where(a => query.Title == null || EF.Functions.ILike(a.Title, $"%{query.Title}%"))
            .WhereIsAccessible(query.Actor)
            .OrderByDescending(a => a.Id)
            .SkipWhile(a => query.Cursor != null && a.Id != query.Cursor)
            .Take(PageSize)
            .Select(a => new AlbumDto()
            {
                Author = a.AuthorId,
                Category = a.CategoryId,
                UpdatedAt = a.UpdatedAt,
                Id = a.Id,
                Tags = a.Tags,
                Title = a.Title,
            })
            .ToArrayAsync(cancellationToken);
    }

    public Task<RemovedAlbumDto[]> GetOrDefaultAsync(
        RemovedAlbumsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Albums.AsNoTracking()
            .Where(a => a.Status == AlbumStatusValue.Removed)
            .Where(a => a.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
            .Select(a => new RemovedAlbumDto
            {
                Id = a.Id,
                Title = a.Title,
                Category = a.CategoryId,
                AccessLevel = a.AccessLevel,
                RemovedAt = a.RemovedAt.GetValueOrDefault(),
            })
            .ToArrayAsync(cancellationToken);
    }
}
