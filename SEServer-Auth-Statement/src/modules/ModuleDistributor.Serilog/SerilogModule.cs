using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace ModuleDistributor.Serilog
{
    public class SerilogModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .ReadFrom.Configuration(context.Configuration)
            .CreateLogger();

            Log.Information("Starting Host.");

            context.Host.UseSerilog();
        }
    }
}