using System;

namespace Game.Controller
{
    /// <summary>
    /// 用于获取Controller的弱引用
    /// 当Controller被销毁或者被回收时，重新获取
    /// </summary>
    public class LazyControllerGetter<T> : LazyGetter<T> where T : BaseComponentController
    {
        public LazyControllerGetter(BaseComponentController controller)
        {
            Owner = controller;
        }

        private BaseComponentController Owner { get; set; }

        public override T Get()
        {
            var value = base.Get();
            if (value != null && value.IsDestroy)
            {
                WeakReference = null;
            }
            else
            {
                return value;
            }
            
            return base.Get();
        }

        protected override T GetInternal()
        {
            return Owner.GetEController<T>();
        }
    }
}