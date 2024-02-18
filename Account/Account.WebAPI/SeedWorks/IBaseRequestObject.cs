using Primitives.Command;
using Shared.Primitives.Query;

namespace Account.WebAPI.SeedWorks
{
    internal interface IBaseRequestObject { }

    internal interface IRequestObject<TProcessRequest> : IBaseRequestObject
        where TProcessRequest : ICommandRequest { }

    internal interface IRequestObject<TProcessRequest, TResponse> : IBaseRequestObject
        where TProcessRequest : IQueryRequest<TResponse> { }
}
