using Microsoft.Extensions.Logging;
using Rougamo;
using Rougamo.Context;

namespace ModuleDistributor.Logging
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExLoggingAttribute : ExMoAttribute
    {
        protected override void ExOnEntry(MethodContext context)
            => context.ResolveLogger().LogInformation($"{context.TargetType.Name}.{context.Method.Name} enter.");

        protected override void ExOnException(MethodContext context)
        {
            context.ResolveLogger().LogCritical(context.Exception, $"{context.TargetType.Name}.{context.Method.Name} throw exception.");
            context.HandledException(this, context.ExReturnType.GetDefaultValue());
        }

        protected override void ExOnExit(MethodContext context)
            => context.ResolveLogger().LogInformation($"{context.TargetType.Name}.{context.Method.Name} exit.");
    }
}
