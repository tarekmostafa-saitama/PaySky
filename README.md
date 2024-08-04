# PaySky Task

This repository contains the PaySky Task project, which is built using ASP.NET Core, SQL Server, Entity Framework Core, Hangfire, MediatR, the Repository Pattern, Serilog, and follows Clean Architecture principles.

## Technologies Used

- **ASP.NET Core**: Web framework for building web applications.
- **SQL Server**: Relational database management system.
- **Entity Framework Core**: Object-database mapper (O/RM) for .NET.
- **Hangfire**: Library to perform background job processing in .NET and .NET Core applications.
- **MediatR**: Simple, unambitious mediator implementation in .NET.
- **Repository Pattern**: Design pattern to encapsulate the logic for accessing data sources.
- **Serilog**: Logging library for structured logging.
- **Clean Architecture**: Architectural principles to maintain separation of concerns and improve maintainability.

## Endpoints Description

### User

- **Create User**
  - Description: Create a new user. User types are 0 (Employee) and 1 (Applicant).
- **Get Tokens**
  - Description: Retrieve access tokens for authentication.
- **Refresh Tokens**
  - Description: Refresh expired tokens.

### Vacancies (Authorized for role Employee)

- **Get Vacancies**
  - Description: Retrieve a list of vacancies.
- **Get All Vacancies (Paginated)**
  - Description: Retrieve all vacancies with pagination.
- **Create Vacancy**
  - Description: Create a new vacancy, launch a domain event to archive it in another table, and delete it when it expires in a scheduled task.
- **Update Vacancy**
  - Description: Update an existing vacancy.
- **Delete Vacancy**
  - Description: Delete a vacancy.

### Applications (Authorized for role Applicant)

- **Create Application**
  - Description: Create a new application. An applicant cannot send more than one application within 24 hours.
- **Search Applications**
  - Description: Search through applications.

## Docker Setup

While Docker has been set up for this project, it is currently not functioning correctly. The project works well using the `paysky.api` profile on port 7253.

To run the project without Docker:
1. Clone the repository.
2. Navigate to the project directory.
3. Ensure that SQL Server is running and the connection string in `appsettings.json` is correctly configured.
4. Run the project using the following command:

   ```bash
   dotnet run --project PaySky.Api --launch-profile "paysky.api"


G##etting Started
To get started with the project:

Clone the repository to your local machine.

Open the solution in your preferred IDE.

Ensure the necessary NuGet packages are restored.

Set up your SQL Server database and update the connection string in appsettings.json.

The database will be automatically migrated during the application startup.

Run the application using the paysky.api profile:
