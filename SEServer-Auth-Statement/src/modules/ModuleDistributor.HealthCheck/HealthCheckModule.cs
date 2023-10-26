using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ModuleDistributor.HealthCheck
{
    public class HealthCheckModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
        {
            context.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
        }

        public override void OnApplicationInitialization(ApplicationContext context)
            => context.EndPoint.MapHealthChecks(responseWriter: UIResponseWriter.WriteHealthCheckUIResponse);
    }
}