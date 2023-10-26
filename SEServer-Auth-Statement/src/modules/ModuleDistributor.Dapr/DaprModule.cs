using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ModuleDistributor.Dapr
{
    public class DaprModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
            => context.Services.Configure<DaprOptions>(context.Configuration.GetSection(nameof(DaprOptions)))
                .AddDaprClient();
    }
}