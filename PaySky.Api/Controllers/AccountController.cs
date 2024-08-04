using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaySky.Application.Common.Models;
using PaySky.Application.Requests.Authentication.Commands;
using PaySky.Application.Requests.Authentication.Models;
using PaySky.Application.Requests.Users.Commands;
using PaySky.Application.Requests.Users.Models;

namespace PaySky.Api.Controllers;

[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{

    [HttpPost("Register")]
    public async Task<ActionResult<Response<AuthenticateResponse>>> Register(CreateUserRequest model)
    {
        var result = await Mediator.Send(new CreateUserCommand(model));
        return Ok(result);
    }

    [HttpPost("GetTokens")]
    public async Task<ActionResult<Response<AuthenticateResponse>>> GetTokens(LoginUserRequest model)
    {
        var result = await Mediator.Send(new LoginUserCommand(model));
        return Ok(result);
    }

    [HttpPost("RefreshTokens")]
    public async Task<ActionResult<AuthenticateResponse>> RefreshTokens(RefreshRequest model)
    {
        var result = await Mediator.Send(new RefreshCommand(model));
        return Ok(result);
    }

    [HttpPost("Logout")]
    [Authorize]
    public async Task<ActionResult<Response<bool>>> Logout()
    {
        await Mediator.Send(new LogOutCommand());
        return Ok(Application.Common.Models.Response.Success(true));
    }
}