using Microsoft.Extensions.Logging;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.Logging
{
    internal static class MethodContextExtensions
    {
        public static ILogger ResolveLogger(this MethodContext context)
        {
            if (context.Target is ILoggerProxy proxy)
                return proxy.Logger;

            foreach (object item in context.Arguments)
            {
                ILogger? logger = item as ILogger;
                if (logger is not null)
                    return logger;
            }

            throw new ArgumentException("Cannot resolve logger. Please check your class whether inherited ILoggerProxy interface or injected ILogger argument.");
        }
    }
}
