using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Identity;
using PaySky.Shared.Permissions;

namespace PaySky.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    private readonly ApplicationDbContext _context = context;


    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var employeeRole = new IdentityRole(DefaultRoles.Employee);
        var applicantRole = new IdentityRole(DefaultRoles.Applicant);

        if (roleManager.Roles.All(r => r.Name != employeeRole.Name))
            await roleManager.CreateAsync(employeeRole);


        if (roleManager.Roles.All(r => r.Name != applicantRole.Name))
            await roleManager.CreateAsync(applicantRole);
     
    }
}