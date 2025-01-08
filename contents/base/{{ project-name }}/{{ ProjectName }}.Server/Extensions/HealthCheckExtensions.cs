using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace {{ ProjectName }}.Server.Extensions;

public static class HealthCheckExtensions
{
    public static IEndpointConventionBuilder MapHealthChecksConfig(
        this IEndpointRouteBuilder endpoints, string pattern)
    {
        return endpoints.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description ?? "No description",
                        duration = e.Value.Duration.ToString()
                    })
                });
                await context.Response.WriteAsync(result);
            }
        });
    }
}