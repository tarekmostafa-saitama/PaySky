using MediatR;
using PaySky.Application.Common.Models;
using PaySky.Application.Requests.Authentication.Models;
using PaySky.Application.Requests.Users.Models;
using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Common.Services;

public interface IIdentityService : IScopedService
{
    Task<IResponse<AuthenticateResponse>> SignInUserASync(LoginUserRequest loginUserRequest);
    Task<IResponse<bool>> SignOutUserASync(string userId);

    Task<IResponse<string>> CreateUserASync(CreateUserRequest createUserRequest);
    Task<IResponse<bool>> AddUserToRoleAsync(string userId, string role);
}