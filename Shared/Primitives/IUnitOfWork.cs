namespace Primitives
{
    public interface IUnitOfWork
    {
        public Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
