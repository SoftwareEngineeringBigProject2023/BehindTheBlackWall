using System;

namespace Game
{
    public abstract class LazyGetter<T> where T : class
    {
        protected WeakReference WeakReference { get; set; }

        public T Value => Get();

        public virtual T Get()
        {
            if (WeakReference == null)
            {
                var target = GetInternal();
                WeakReference = new WeakReference(target, false);
                return target;
            }

            if (WeakReference.Target == null)
            {
                var target = GetInternal();
                WeakReference.Target = target;
                return target;
            }

            return WeakReference.Target as T;
        }

        protected abstract T GetInternal();
    }
}