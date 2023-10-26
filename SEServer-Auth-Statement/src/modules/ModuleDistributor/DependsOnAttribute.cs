using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DependsOnAttribute : Attribute
    {
        private static readonly Type _type = typeof(CustomModule);

        public IEnumerable<Type> Modules { get; }

        public DependsOnAttribute(params Type[] modules)
            => Modules = modules.Where(item => _type.IsAssignableFrom(item));
    }
}
