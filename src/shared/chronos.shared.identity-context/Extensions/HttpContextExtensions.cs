using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http;

public static class HttpContextExtensions
{
    private const string EmployeeIdHeader = "X-Employee-Id";

    public static Ulid? GetEmployeeContext(this HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue(EmployeeIdHeader, out var employeeIdValue))
        {
            return null;
        }

        var employeeIdString = employeeIdValue.ToString();

        if (string.IsNullOrWhiteSpace(employeeIdString))
        {
            return null;
        }

        if (Ulid.TryParse(employeeIdString, out var employeeId))
        {
            return employeeId;
        }

        return null;
    }
}
