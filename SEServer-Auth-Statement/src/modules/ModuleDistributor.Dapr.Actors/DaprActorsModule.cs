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
                var param1 = Expression.Parameter(typeof(ActorRegistrationCollection));
                var param2 = Expression.Parameter(typeof(Action<ActorRegistration>));

                List<Expression> list = new List<Expression>();
                var method = typeof(ActorRegistrationCollection)
                    .GetMethods()
                    .FirstOrDefault(item => item.Name == "RegisterActor"
                        && item.GetParameters().Length <= 1);

                if (method is null)
                    throw new ArgumentNullException(nameof(method));

                foreach (var item in assembly.GetTypes())
                    if (item.IsSubclassOf(type))
                    {
                        var temp = method.MakeGenericMethod(item);
                        list.Add(Expression.Call(param1, temp, param2));
                    }
                        
                Expression.Lambda<Action<ActorRegistrationCollection, Action<ActorRegistration>?>>(Expression.Block(list), param1, param2)
                    .Compile()
                    .Invoke(options.Actors, null);
            });
        }

        public override void OnApplicationInitialization(ApplicationContext context)
            => context.EndPoint.MapActorsHandlers();
    }
}