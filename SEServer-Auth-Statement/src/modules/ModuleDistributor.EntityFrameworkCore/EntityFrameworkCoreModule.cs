using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ModuleDistributor.EntityFrameworkCore
{
    public class EntityFrameworkCoreModule<TDbContext, TWrapper> : CustomModule
        where TDbContext : DbContext where TWrapper : OptionsActionWrapper, new()
    {
        public override void ConfigureServices(ServiceContext context)
        {
            OptionsActionWrapper wrapper = new TWrapper();
            context.Services.AddDbContext<TDbContext>(wrapper.OptionsAction, wrapper.ContextLifetime, wrapper.OptionsLifetime);
        }
    }
}