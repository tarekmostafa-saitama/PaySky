using Microsoft.AspNetCore.Identity;
using PaySky.Domain.Enums;

namespace PaySky.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    public UserType UserType { get; set; }
}