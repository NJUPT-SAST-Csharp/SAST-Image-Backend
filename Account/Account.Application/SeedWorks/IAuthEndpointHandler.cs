using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Account.Application.SeedWorks
{
    public interface IAuthEndpointHandler<TRequest>
        where TRequest : IRequest
    {
        public Task<IResult> Handle(TRequest request, ClaimsPrincipal user);
    }
}
