using Dapr.Actors.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace ModuleDistributor.Dapr.Actors
{
    [DependsOn(typeof(DaprModule))]
    public class DaprActorsModule<TEntryModule> : CustomModule
        where TEntryModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
        {
            Assembly assembly = typeof(TEntryModule).Assembly;

            context.Services.AddActors(options =>
            {
                var type = typeof(Actor);
                var param = Expression.Parameter(typeof(ActorRegistrationCollection));
                List<Expression> list = new List<Expression>();

                foreach (var item in assembly.GetTypes())
                    if (item.IsSubclassOf(type))
                        list.Add(Expression.Call(param, "RegisterActor", new Type[] { item }));
                
                Expression.Lambda<Action<ActorRegistrationCollection>>(Expression.Block(list), param)
                    .Compile()
                    .Invoke(options.Actors);
            });
        }

        public override void OnApplicationInitialization(ApplicationContext context)
            => context.EndPoint.MapActorsHandlers();
    }
}