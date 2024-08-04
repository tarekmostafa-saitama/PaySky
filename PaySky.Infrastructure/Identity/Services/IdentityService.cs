using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaySky.Application.Common.Authentication;
using PaySky.Application.Common.Models;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Application.Requests.Authentication.Models;
using PaySky.Application.Requests.Users.Models;
using PaySky.Domain.Entities;
using PaySky.Shared.Exceptions;

namespace PaySky.Infrastructure.Identity.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IAuthenticateService authenticateService,
    IUnitOfWork unitOfWork)
    : IIdentityService
{
    public async Task<IResponse<AuthenticateResponse>> SignInUserASync(LoginUserRequest loginUserRequest)
    {
        var user = await userManager.FindByEmailAsync(loginUserRequest.Email);
        if (user == null)
            return Response.Fail<AuthenticateResponse>("Failed login attempt");
        var signInResult =
            await signInManager.PasswordSignInAsync(user, loginUserRequest.Password, false, false);

        return signInResult.Succeeded
            ? Response.Success(await authenticateService.Authenticate(user.Id, CancellationToken.None))
            : Response.Fail<AuthenticateResponse>("Failed login attempt");
    }

    public async Task<IResponse<bool>> SignOutUserASync(string userId)
    {
        await signInManager.SignOutAsync();
        unitOfWork.RefreshTokensRepository.RemoveRange(x => x.UserId == userId);
        await unitOfWork.CommitAsync();
        return Response.Success(true);
    }






    public async Task<IResponse<string>> CreateUserASync(CreateUserRequest createUserRequest)
    {
        var user = new ApplicationUser
        {
            Email = createUserRequest.Email,
            FullName = createUserRequest.FullName,
            UserName = createUserRequest.Email,
            UserType = createUserRequest.UserType
        };
        var createResult = await userManager.CreateAsync(user, createUserRequest.Password);
        return createResult.Succeeded
            ? Response.Success(user.Id)
            : Response.Fail<string>(createResult.Errors.Select(error => error.Description).ToList());
    }

    public async Task<IResponse<bool>> AddUserToRoleAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        var result = await userManager.AddToRoleAsync(user, role);
        return result.Succeeded
        ? Response.Success(true)
            : Response.Fail<bool>(result.Errors.ToList().First().Description);
    }
}