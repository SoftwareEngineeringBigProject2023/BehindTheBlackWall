using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.DependencyInjection
{
    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly)
        {
            return services.ResolveFromAttribute(assembly);
        }

        private static IServiceCollection ResolveFromAttribute(this IServiceCollection services, Assembly assembly)
        {
            foreach (var item in assembly.GetTypes())
            {
                foreach(var attribute in item.GetCustomAttributes())
                {
                    SingletonAttribute? singleton = attribute as SingletonAttribute;
                    if (singleton is not null)
                    {
                        if (singleton.InterfaceType is not null)
                            services.AddSingleton(singleton.InterfaceType, item);
                        else
                            services.AddSingleton(item);
                        break;
                    }

                    ScopedAttribute? scoped = attribute as ScopedAttribute;
                    if (scoped is not null)
                    {
                        if (scoped.InterfaceType is not null)
                            services.AddScoped(scoped.InterfaceType, item);
                        else
                            services.AddScoped(item);
                        break;
                    }

                    TransientAttribute? transient = attribute as TransientAttribute;
                    if (transient is not null)
                    {
                        if (transient.InterfaceType is not null)
                            services.AddTransient(transient.InterfaceType, item);
                        else
                            services.AddTransient(item);
                        break;
                    }
                }
            }
            return services;
        }
    }
}
