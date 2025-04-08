using MediatR;

namespace APPLICATION.Abstractions.Messagin;

public interface IQuery<TResponse> : IRequest<TResponse> { }
