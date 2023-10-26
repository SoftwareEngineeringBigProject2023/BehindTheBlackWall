using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor
{
    public static class WebApplicationExtensions
    {
        public static async ValueTask ConfigureServiceAsync<TModule>(this WebApplicationBuilder builder) where TModule : CustomModule, new()
        {
            LinkedList<CustomModule> modules = new LinkedList<CustomModule>();
            HashSet<Type> types = new HashSet<Type>();  
            AddAllModule(modules, types, typeof(TModule));
            foreach (var module in modules)
                await module.ConfigureServicesAsync(new ServiceContext(builder));
            builder.Services.AddSingleton(new ModuleDependencyWrapper(modules));
        }

        private static void AddAllModule(LinkedList<CustomModule> modules, HashSet<Type> types, Type type)
        {
            types.Add(type);
            var attribute = type.GetCustomAttribute<DependsOnAttribute>();

            if (attribute is not null)
                foreach (var item in attribute.Modules)
                    if (!types.Contains(item))
                        AddAllModule(modules, types, item);

            if (Activator.CreateInstance(type) is CustomModule module)
                modules.AddLast(module);
        }

        public static async ValueTask OnApplicationInitializationAsync(this WebApplication app)
        {
            foreach (var module in app.Services.GetService<ModuleDependencyWrapper>()!.Modules)
                await module.OnApplicationInitializationAsync(new ApplicationContext(app));
        }
    }
}
