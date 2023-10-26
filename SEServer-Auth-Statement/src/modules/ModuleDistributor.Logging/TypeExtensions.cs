using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.Logging
{
#nullable disable
    internal static class TypeExtensions
    {
        private static readonly Type _voidType = typeof(void);

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) is null && type != _voidType)
                return Activator.CreateInstance(type);
            else
                return null;
        }
    }
}
