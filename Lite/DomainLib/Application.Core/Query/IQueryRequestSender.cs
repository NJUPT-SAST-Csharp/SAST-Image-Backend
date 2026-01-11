namespace Application.Query;

public interface IQueryRequestSender
{
    public Task<TResult> SendAsync<TResult>(
        IQuery<TResult> request,
        CancellationToken cancellationToken = default
    );
}
