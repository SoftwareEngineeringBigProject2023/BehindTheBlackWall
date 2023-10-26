using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TransientAttribute : Attribute
    {
        public virtual Type? InterfaceType { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TransientAttribute<TInterface> : TransientAttribute
    {
        public override Type? InterfaceType { get; } = typeof(TInterface);
    }
}
