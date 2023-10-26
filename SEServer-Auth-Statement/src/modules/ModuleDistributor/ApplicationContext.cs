using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor
{
    public sealed class ApplicationContext
    {
        public IApplicationBuilder App { get; }
        
        public IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }
        
        public IEndpointRouteBuilder EndPoint { get; }

        public ApplicationContext(WebApplication app)
        {
            App = app;
            Configuration = app.Configuration;
            Environment = app.Environment;
            EndPoint = app;
        }
    }
}
