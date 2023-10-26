using Microsoft.Extensions.Logging;
using Rougamo;
using Rougamo.Context;

namespace ModuleDistributor.Logging
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LoggingAttribute : MoAttribute
    {
        public override void OnEntry(MethodContext context)
            => context.ResolveLogger().LogInformation($"{context.TargetType.Name}.{context.Method.Name} enter.");

        public override void OnException(MethodContext context)
        {
            context.ResolveLogger().LogCritical(context.Exception, $"{context.TargetType.Name}.{context.Method.Name} throw exception.");
            context.HandledException(this, context.RealReturnType.GetDefaultValue());
        }

        public override void OnExit(MethodContext context)
            => context.ResolveLogger().LogInformation($"{context.TargetType.Name}.{context.Method.Name} exit.");

    }
}
