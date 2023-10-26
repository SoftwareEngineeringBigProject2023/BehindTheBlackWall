namespace ModuleDistributor.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SingletonAttribute : Attribute
    {
        public virtual Type? InterfaceType { get; } 
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SingletonAttribute<TInterface> : SingletonAttribute
    {
        public override Type? InterfaceType { get; } = typeof(TInterface);
    }
}