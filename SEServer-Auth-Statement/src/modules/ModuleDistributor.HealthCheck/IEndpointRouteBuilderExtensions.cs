using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ModuleDistributor.HealthCheck
{
#nullable disable
    internal static class IEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapHealthChecks(this IEndpointRouteBuilder endPoint, string healthPattern = "/hc",
            string livenessPattern = "/liveness", Func<HttpContext, HealthReport, Task> responseWriter = default)
        {
            endPoint.MapHealthChecks(healthPattern, new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = responseWriter,
            });

            
            endPoint.MapHealthChecks(livenessPattern, new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            return endPoint;
        }
    }
}
