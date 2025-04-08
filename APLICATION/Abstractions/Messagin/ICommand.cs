using DOMAIN.SharedKernel.Primitives;
using MediatR;

namespace APPLICATION.Abstractions.Messagin;

public interface ICommand : IRequest<Result>, ICommandBase { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase { }

public interface ICommandBase { }
