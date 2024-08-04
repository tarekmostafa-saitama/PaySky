using PaySky.Domain.Common;
using PaySky.Domain.ValueObjects;

namespace PaySky.Application.Requests.Vacancies.Models;

public class VacancyVm : Aggregate<VacancyId>
{
    private readonly List<Domain.Entities.Application> _applications = new();
    public string Title { get; set; }
    public string Description { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }
    public string BackgroundJobId { get; set; }
    public int Salary { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public int MaxApplications { get; set; }
    public bool IsActive { get; set; }
    public List<Domain.Entities.Application> Applications { get; set; }

    public IReadOnlyList<Domain.Entities.Application> LoginAttempts => _applications.AsReadOnly();
}