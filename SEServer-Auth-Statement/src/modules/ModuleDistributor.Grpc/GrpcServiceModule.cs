using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;
using System.Reflection;

namespace ModuleDistributor.Grpc
{
    [DependsOn(typeof(GrpcModule))]
    public class GrpcServiceModule<TEntryModule> : CustomModule
        where TEntryModule : CustomModule
    {
        public override void OnApplicationInitialization(ApplicationContext context)
        {
            Assembly assembly = typeof(TEntryModule).Assembly;
            List<Expression> list = new List<Expression>();
            List<ParameterExpression> variables = new List<ParameterExpression>();
            var param = Expression.Parameter(typeof(IEndpointRouteBuilder));
            var method = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService",
                BindingFlags.Public | BindingFlags.Static);

            if (method is null)
                throw new ArgumentNullException(nameof(method));

            foreach (var item in assembly.GetTypes())
                if (item.GetCustomAttribute<GrpcServiceAttribute>() is not null)
                {
                    var temp = method.MakeGenericMethod(item);
                    list.Add(Expression.Call(temp, param));
                }

            Expression.Lambda<Action<IEndpointRouteBuilder>>(Expression.Block(list), param)
                .Compile()
                .Invoke(context.EndPoint);
        }
    }
}
