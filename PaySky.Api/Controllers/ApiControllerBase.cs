using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PaySky.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}