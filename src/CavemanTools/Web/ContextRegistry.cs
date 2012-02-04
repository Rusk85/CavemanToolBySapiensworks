using System;
using System.Management.Instrumentation;
using System.Web;

namespace CavemanTools.Web
{
    public abstract class ContextRegistry<T> where T : class
    {
        public const string ContextKey = "_context-info_";
        
        public static T Instance
        {
            get
            {
                var inst = HttpContext.Current.Items[ContextKey] as T;
                if (inst == null)
                {
                    throw new InstanceNotFoundException(string.Format("An instance of {0} was not found", typeof(T).Name));
                }
                return inst;
            }
        }

        public static void Register(T inst)
        {
            if (inst == null) throw new ArgumentNullException("inst");
            if (HttpContext.Current.Items.Contains(ContextKey)) throw new InvalidOperationException("There is an instance registered already");
            HttpContext.Current.Items[ContextKey] = inst;
        }
    }
}