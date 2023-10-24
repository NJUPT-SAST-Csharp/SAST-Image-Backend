namespace SastImg.Domain
{
    public interface IUnitOfWork
    {
        public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
