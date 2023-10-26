using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ModuleDistributor.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.HealthCheck.Dapr
{
    [DependsOn(typeof(DaprModule))]
    public class DaprHealthCheckModule : HealthCheckModule
    {
        public override void ConfigureServices(ServiceContext context)
        {
            context.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddCheck<DaprHealthCheck>("dapr");
        }
    }
}
