using System;
using System.Management.Instrumentation;
using System.Web;

namespace CavemanTools.Web
{
    public abstract class ContextInstance<T> where T : class
    {
        public const string ContextKey = "_context-info_";
        
        static object _lock=new object();
        public static T Instance
        {
            get
            {
                var key = ContextKey + (typeof(T).Name);
                var inst = HttpContext.Current.Items[key] as T;
                if (inst == null)
                {
                    throw new InstanceNotFoundException(string.Format("An instance of {0} was not found", typeof(T).Name));
                }
                return inst;
            }
        }

        protected void Register(T inst)
        {
            if (inst == null) throw new ArgumentNullException("inst");
            var key = ContextKey + (inst.GetType().Name);
            lock (_lock)
            {
                //if (HttpContext.Current.Items.Contains(key)) throw new InvalidOperationException("There is an instance registered already");
                if (!HttpContext.Current.Items.Contains(key)) HttpContext.Current.Items[key] = inst;
            }
        }
    }
}