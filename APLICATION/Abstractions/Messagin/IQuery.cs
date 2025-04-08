using MediatR;

namespace APLICATION.Abstractions.Messagin
{
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}
