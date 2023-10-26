using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScopedAttribute : Attribute
    {
        public virtual Type? InterfaceType { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScopedAttribute<TInterface> : ScopedAttribute
    {
        public override Type? InterfaceType { get; } = typeof(TInterface);
    }
}
