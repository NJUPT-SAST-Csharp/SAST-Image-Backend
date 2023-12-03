using Microsoft.AspNetCore.Http;

namespace Account.Application.SeedWorks
{
    public interface IEndpointHandler<T>
        where T : IRequest
    {
        Task<IResult> Handle(T request);
    }
}
