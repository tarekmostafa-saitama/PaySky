using MediatR;

namespace PaySky.Shared.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
{
}