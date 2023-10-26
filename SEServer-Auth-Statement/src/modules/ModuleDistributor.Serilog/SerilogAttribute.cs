using Rougamo;
using Rougamo.Context;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ModuleDistributor.Serilog
{
    public class SerilogAttribute : ExMoAttribute
    {
        protected override void ExOnException(MethodContext context)
        {
            if (context.Exception is null || context.Exception is HostAbortedException)
                return;

            Log.Error(context.Exception, "Host terminated unexpectedly!");
            context.HandledException(this, 1);
        }

        protected override void ExOnExit(MethodContext context)
            => Log.CloseAndFlush();
    }
}
