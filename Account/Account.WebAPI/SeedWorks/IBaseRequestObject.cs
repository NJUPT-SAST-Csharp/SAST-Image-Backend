using Primitives.Command;
using Shared.Primitives.Query;

namespace Account.WebAPI.SeedWorks
{
    internal interface IBaseRequestObject { }

    internal interface ICommandRequestObject<TProcessRequest> : IBaseRequestObject
        where TProcessRequest : ICommandRequest { }

    internal interface ICommandRequestObject<TProcessRequest, TResponse> : IBaseRequestObject
        where TProcessRequest : ICommandRequest<TResponse> { }

    internal interface IQueryRequestObject<TProcessRequest, TResponse> : IBaseRequestObject
        where TProcessRequest : IQueryRequest<TResponse> { }
}
