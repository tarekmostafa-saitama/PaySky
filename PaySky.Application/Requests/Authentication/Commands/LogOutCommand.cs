using MediatR;
using PaySky.Application.Common.Models;
using PaySky.Application.Common.Services;

namespace PaySky.Application.Requests.Authentication.Commands;

public record LogOutCommand : IRequest<IResponse<bool>>;

internal sealed class LogOutCommandHandler(ICurrentUserService currentUserService,
        IIdentityService identityService)
    : IRequestHandler<LogOutCommand, IResponse<bool>>
{
    public async Task<IResponse<bool>> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.Id;
        return await identityService.SignOutUserASync(userId);
    }
}