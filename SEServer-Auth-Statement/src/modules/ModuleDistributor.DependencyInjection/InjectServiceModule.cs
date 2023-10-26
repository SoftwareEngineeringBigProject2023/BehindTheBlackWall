using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.DependencyInjection
{
    public class InjectServiceModule<TEntryModule> : CustomModule
        where TEntryModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
            => context.Services.AddAllServices(typeof(TEntryModule).Assembly);
    }
}
