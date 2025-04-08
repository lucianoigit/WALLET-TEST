using DOMAIN.SharedKernel.Primitives;
using MediatR;

namespace APLICATION.Abstractions.Messagin
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand { }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse> { }

}
