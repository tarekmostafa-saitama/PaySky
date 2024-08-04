using MediatR;

namespace PaySky.Shared.CQRS;

public interface ICommand : IRequest<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}