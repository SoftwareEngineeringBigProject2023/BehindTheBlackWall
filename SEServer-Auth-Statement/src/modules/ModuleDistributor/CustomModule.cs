using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ModuleDistributor
{
    public abstract class CustomModule
    {
        public virtual ValueTask ConfigureServicesAsync(ServiceContext context)
        {
            ConfigureServices(context);
            return ValueTask.CompletedTask;
        }

        public virtual void ConfigureServices(ServiceContext context)
        {

        }

        public virtual ValueTask OnApplicationInitializationAsync(ApplicationContext context)
        {
            OnApplicationInitialization(context);
            return ValueTask.CompletedTask;
        }

        public virtual void OnApplicationInitialization(ApplicationContext context)
        {

        }
    }
}