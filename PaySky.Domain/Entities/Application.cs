using PaySky.Domain.Common;
using PaySky.Domain.ValueObjects;
using ApplicationId = PaySky.Domain.ValueObjects.ApplicationId;

namespace PaySky.Domain.Entities;

public class Application : Entity<ValueObjects.ApplicationId>
{
    public VacancyId VacancyId { get; set; }
    public string ApplicantId { get; set; }


    public Vacancy Vacancy { get; set; }

    public static Application Create(ApplicationId id, VacancyId vacancyId)
    {

        var application = new Application
        {
            Id = id,
            VacancyId = vacancyId
           
        };
        return application;
    }

}