namespace SastImg.Domain
{
    public interface IUnitOfWork
    {
        public Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
