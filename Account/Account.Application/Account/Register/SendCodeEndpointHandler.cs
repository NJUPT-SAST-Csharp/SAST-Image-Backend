using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Http.HttpResults;
using Response.ReponseObjects;
using Shared.Response.Builders;

namespace Account.Application.Account.Register
{
    public sealed class SendCodeEndpointHandler : IEndpointHandler
    {
        public async Task<Results<NoContent, BadRequest<BadRequestResponse>>> Handler(
            SendCodeRequest request
        )
        {
            return Responses.NoContent;
        }
    }
}
