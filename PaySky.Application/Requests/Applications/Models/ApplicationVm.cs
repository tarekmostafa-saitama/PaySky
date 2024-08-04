using PaySky.Domain.Common;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;
using ApplicationId = PaySky.Domain.ValueObjects.ApplicationId;

namespace PaySky.Application.Requests.Applications.Models;

public class ApplicationVm : Entity<ApplicationId>
{
    public VacancyId VacancyId { get; set; }
    public string ApplicantId { get; set; }

    public Vacancy Vacancy { get; set; }


}