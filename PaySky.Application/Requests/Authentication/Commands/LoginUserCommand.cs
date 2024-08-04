using MediatR;
using PaySky.Application.Common.Models;
using PaySky.Application.Common.Services;
using PaySky.Application.Requests.Authentication.Models;

namespace PaySky.Application.Requests.Authentication.Commands;

public record LoginUserCommand(LoginUserRequest LoginUserRequest) : IRequest<IResponse<AuthenticateResponse>>;

internal sealed class LoginUserCommandHandler(IIdentityService identityService) : IRequestHandler<LoginUserCommand, IResponse<AuthenticateResponse>>
{
    public async Task<IResponse<AuthenticateResponse>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        return await identityService.SignInUserASync(request.LoginUserRequest);
    }
}