using Hangfire.Dashboard;

namespace PaySky.Infrastructure.BackgroundJobs;

public class HangFireFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return true;
        // Allow all authenticated users to see the Dashboard (potentially dangerous).
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}