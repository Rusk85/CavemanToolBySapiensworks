using System;
using System.Management.Instrumentation;
using System.Web;

namespace CavemanTools.Web
{
    /// <summary>
    /// Base class for types which will be used as singletons within a http request
    /// </summary>
    /// <typeparam name="T">Reference type</typeparam>
    public abstract class ContextInstance<T> where T : class
    {
        public const string ContextKey = "_context-info_";
        
        /// <summary>
        /// Gets the single instance of type for the current request
        /// </summary>
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

        /// <summary>
        /// Register an instance of T to http context as a request scoped singleton
        /// </summary>
        /// <param name="inst"></param>
        protected void Register(T inst)
        {
            if (inst == null) throw new ArgumentNullException("inst");
            var key = ContextKey + (inst.GetType().Name);
            
            lock (HttpContext.Current.Items.SyncRoot)
            {
                //if (HttpContext.Current.Items.Contains(key)) throw new InvalidOperationException("There is an instance registered already");
                if (!HttpContext.Current.Items.Contains(key)) HttpContext.Current.Items[key] = inst;
            }
        }
    }
}