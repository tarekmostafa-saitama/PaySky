using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaySky.Application.Requests.Vacancies.Commands;
using PaySky.Application.Requests.Vacancies.Models;
using PaySky.Application.Requests.Vacancies.Queries;
using PaySky.Shared.Permissions;

namespace PaySky.Api.Controllers;

[Authorize(Roles = DefaultRoles.Employee)]
[Route("api/[controller]")]

public class VacanciesController : ApiControllerBase
{
    /// <summary>
    ///     Lists all vacancies with pagination and search functionality.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="searchValue">The search term to filter vacancies.</param>
    /// <returns>A list of vacancies.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<VacancyVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(int page, int size, string searchValue)
    {
        var vacancies = await Mediator.Send(new GetVacanciesQuery(page, size, searchValue));
        return Ok(vacancies);
    }

    /// <summary>
    ///     Retrieves a specific vacancy by ID.
    /// </summary>
    /// <param name="vacancyId">The ID of the vacancy to retrieve.</param>
    /// <returns>The requested vacancy.</returns>
    [ProducesResponseType(typeof(VacancyVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet(("{vacancyId}"))]
    public async Task<IActionResult> GetVacancy(Guid vacancyId)
    {
        var vacancies = await Mediator.Send(new GetVacancyQuery(vacancyId));
        return Ok(vacancies);
    }


    /// <summary>
    ///     Creates a new vacancy.
    /// </summary>
    /// <param name="vacancyVm">The vacancy details.</param>
    /// <returns>The created vacancy.</returns>
    [ProducesResponseType(typeof(VacancyVm), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Create(VacancyVm vacancyVm)
    {
        var vacancy = await Mediator.Send(new CreateVacancyCommand(vacancyVm));
        return Ok(vacancy);
    }

    /// <summary>
    ///     Updates an existing vacancy.
    /// </summary>
    /// <param name="vacancyId">The ID of the vacancy to update.</param>
    /// <param name="vacancyVm">The updated vacancy details.</param>
    /// <returns>The updated vacancy.</returns>
    [HttpPut("{vacancyId}")]
    [ProducesResponseType(typeof(VacancyVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid vacancyId, VacancyVm vacancyVm)
    {
        var result = await Mediator.Send(new UpdateVacancyCommand(vacancyId, vacancyVm));
        return Ok(result);
    }

    /// <summary>
    ///     Deletes a vacancy by ID.
    /// </summary>
    /// <param name="vacancyId">The ID of the vacancy to delete.</param>
    /// <returns>A confirmation of deletion.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{vacancyId}")]
    public async Task<IActionResult> Delete(Guid vacancyId)
    {
        var result = await Mediator.Send(new DeleteVacancyCommand(vacancyId));
        return Ok(result);
    }

    /// <summary>
    ///     Toggles the active status of a vacancy.
    /// </summary>
    /// <param name="vacancyId">The ID of the vacancy to update.</param>
    /// <param name="status">The new active status.</param>
    /// <returns>The updated vacancy status.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{vacancyId}/toggle-active")]
    public async Task<IActionResult> ToggleActiveVacancy(Guid vacancyId, bool status)
    {
        var result = await Mediator.Send(new ToggleVacancyStatusCommand(vacancyId, status));
        return Ok(result);
    }
}