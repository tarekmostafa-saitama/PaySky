using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaySky.Application.Requests.Applications.Commands;
using PaySky.Application.Requests.Applications.Queries;
using PaySky.Shared.Permissions;

namespace PaySky.Api.Controllers
{

    [Authorize(Roles = DefaultRoles.Applicant)]
    [Route("api/[controller]")]
    public class ApplicationsController : ApiControllerBase
    {
        [HttpPost("{vacancyId}")]
        public async Task<IActionResult> Create(Guid vacancyId)
        {
            var application = await Mediator.Send(new CreateApplicationCommand(vacancyId));
            return Ok(application);
        }


        [HttpGet]
        public async Task<IActionResult> Search(int page, int size, string searchValue)
        {
            var applications = await Mediator.Send(new SearchForApplicationsQuery(page , size, searchValue));
            return Ok(applications);
        }
    }
}
