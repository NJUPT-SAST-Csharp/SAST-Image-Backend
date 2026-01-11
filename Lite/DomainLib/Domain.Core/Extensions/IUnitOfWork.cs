namespace Domain.Extensions;

public interface IUnitOfWork 
{
    public Task CommitChangesAsync(CancellationToken cancellationToken = default);
}
