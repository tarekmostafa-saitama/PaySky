using PaySky.Domain.Common;
using PaySky.Domain.DomainEvents;
using PaySky.Domain.ValueObjects;

namespace PaySky.Domain.Entities;

public class Vacancy : Aggregate<VacancyId>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Company { get; private set; }
    public string Location { get; private set; }
    public string BackgroundJobId { get; private set; }
    public int Salary { get; private set; }
    public DateTimeOffset ExpirationDate { get; private set; }
    public int MaxApplications { get; private set; }
    public bool IsActive { get; private set; }

    public string EmployeeId { get; private set; }

    public List<Application> Applications { get; private set; }

    public static Vacancy Create(VacancyId id, string title, string description, string company, string location,
        int salary, DateTimeOffset expirationDate, int maxApplications, string backgroundJobId, string employeeId, bool isActive)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);
        ArgumentException.ThrowIfNullOrWhiteSpace(location);


        var vacancy = new Vacancy
        {
            Id = id,
            Title = title,
            Description = description,
            Company = company,
            Location = location,
            Salary = salary,
            ExpirationDate = expirationDate,
            BackgroundJobId = backgroundJobId,
            MaxApplications = maxApplications,
            EmployeeId = employeeId,
            IsActive = isActive
        };
        vacancy.AddDomainEvent(new VacancyCreatedEvent(vacancy));
        return vacancy;
    }

    public void Update(string title, string description, string company, string location, int salary,
        DateTimeOffset expirationDate, int maxApplications,  bool isActive)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);
        ArgumentException.ThrowIfNullOrWhiteSpace(location);


        Title = title;
        Description = description;
        Company = company;
        Location = location;
        Salary = salary;
        ExpirationDate = expirationDate;
        MaxApplications= maxApplications;
        IsActive = isActive;

        AddDomainEvent(new VacancyUpdatedEvent(this));
    }

    public void UpdateIsActiveStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void SetJobId(string jobId)
    {
        BackgroundJobId = jobId;
    }
}