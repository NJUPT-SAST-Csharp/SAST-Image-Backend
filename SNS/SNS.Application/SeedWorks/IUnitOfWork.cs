namespace SNS.Application.SeedWorks
{
    public interface IUnitOfWork
    {
        public Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
