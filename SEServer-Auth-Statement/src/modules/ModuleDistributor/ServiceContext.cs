using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor
{
    public sealed class ServiceContext
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        public IConfigurationBuilder ConfigurationBuilder { get; }

        public IHostBuilder Host { get; }
 
        public ServiceContext(WebApplicationBuilder builder)
        {
            Services = builder.Services;
            Configuration = builder.Configuration;
            Host = builder.Host;
            ConfigurationBuilder = builder.Configuration;
        }
    }
}
