using MediatR;
using PaySky.Application.Common.Models;
using PaySky.Application.Common.Services;
using PaySky.Application.Requests.Users.Models;
using PaySky.Domain.Enums;
using PaySky.Shared.Permissions;

namespace PaySky.Application.Requests.Users.Commands;

public record CreateUserCommand(CreateUserRequest RegisterUserRequest) : IRequest<IResponse<string>>;

internal sealed class RegisterUserCommandHandler(IIdentityService identityService) : IRequestHandler<CreateUserCommand, IResponse<string>>
{
    public async Task<IResponse<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result =  await identityService.CreateUserASync(request.RegisterUserRequest);
        if (result.Succeeded)
        {
            switch (request.RegisterUserRequest.UserType)
            {
                case UserType.Employee:
                    await identityService.AddUserToRoleAsync(result.Data, DefaultRoles.Employee);
                    break;
                case UserType.Applicant:
                    await identityService.AddUserToRoleAsync(result.Data, DefaultRoles.Applicant);
                    break;
            }
        }
        return result;
    }
}