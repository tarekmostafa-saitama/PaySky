using PaySky.Domain.Common;
using PaySky.Domain.ValueObjects;

namespace PaySky.Domain.Entities;

public class ArchivedVacancy : Aggregate<VacancyId>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Company { get; private set; }
    public string Location { get; private set; }
    public int Salary { get; private set; }
    public DateTimeOffset ExpirationDate { get; private set; }
    public int MaxApplications { get; private set; }
    public string EmployeeId { get; private set; }

    public List<Application> Applications { get; private set; }

    public static ArchivedVacancy Create(VacancyId id, string title, string description, string company, string location,
        int salary, DateTimeOffset expirationDate, int maxApplications, string employeeId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);
        ArgumentException.ThrowIfNullOrWhiteSpace(location);


        var archivedVacancy = new ArchivedVacancy
        {
            Id = id,
            Title = title,
            Description = description,
            Company = company,
            Location = location,
            Salary = salary,
            ExpirationDate = expirationDate,
            MaxApplications = maxApplications,
            EmployeeId = employeeId,
        };
        return archivedVacancy;
    }

  
}