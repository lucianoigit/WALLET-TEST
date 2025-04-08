using MediatR;

namespace APPLICATION.Abstractions.Messagin;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{ }
