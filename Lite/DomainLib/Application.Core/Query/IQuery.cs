using MediatR;

namespace Application.Query;

public interface IQuery<TResult> : IRequest<TResult> { }
