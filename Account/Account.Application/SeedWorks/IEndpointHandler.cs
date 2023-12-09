using Microsoft.AspNetCore.Http;

namespace Account.Application.SeedWorks
{
    public interface IEndpointHandler<TRequest>
        where TRequest : IRequest
    {
        Task<IResult> Handle(TRequest request);
    }
}
