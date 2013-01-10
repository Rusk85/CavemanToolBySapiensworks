using System;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using CavemanTools.Infrastructure;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace CavemanTools.Web
{
    public class AspNetCache:ICacheData
    {
     class CacheDependencyWrapper:CacheDependency
     {
         private readonly ChangeMonitor _monitor;

         public CacheDependencyWrapper(ChangeMonitor monitor)
         {
             _monitor = monitor; 
             
             monitor.NotifyOnChanged(state=> this.NotifyDependencyChanged(this,EventArgs.Empty));
         }

         
     }
        
        private Cache _cache;

        public AspNetCache()
            : this(HttpRuntime.Cache)
        {
            
        }

        internal AspNetCache(Cache cache)
        {
            _cache = cache;            
        }


        public bool Contains(string key)
        {
            key.MustNotBeNull();
            return _cache.Get(key) != null;
        }

        public object Get(string key, DateTimeOffset? newExpiration = null)
        {
            key.MustNotBeNull();
            var rez = _cache.Get(key);
            if (rez != null)
            {
                if (newExpiration.HasValue)
                {
                    Set(key,rez,newExpiration.Value);
                }
            }
            return rez;
        }

        /// <summary>
        /// Gets typed object from cache or the supplied default value if the value doesn't exist.
        /// Optionally set a new expiration date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue">Value to return if the object doesn't exist in cache</param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue = default(T), DateTimeOffset? newExpiration = null)
        {
            var rez = Get(key, newExpiration);
            if (rez != null)
            {
                return (T)rez;
            }
            return defaultValue;
        }

        /// <summary>
        /// Tries to add value to cache.
        /// Returns false if the value already exists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="monitor"></param>
        /// <returns></returns>
        public object Add(string key, object value, TimeSpan slidingExpiration, ChangeMonitor monitor = null)
        {
            key.MustNotBeNull();
            value.MustNotBeNull();
            CacheDependency dep=null;
            if (monitor != null)
            {
                dep= new CacheDependencyWrapper(monitor);
            }

            var old=_cache.Add(key, value, dep, Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.Default, null);
            return old ;
        }

        /// <summary>
        /// Tries to add value to cache.
        /// Returns false if the value already exists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="monitor"></param>
        public object Add(string key, object value, DateTimeOffset absoluteExpiration, ChangeMonitor monitor = null)
        {
            key.MustNotBeNull();
            value.MustNotBeNull();
            CacheDependency dep = null;
            if (monitor != null)
            {
                dep = new CacheDependencyWrapper(monitor);
            }

            var old = _cache.Add(key, value, dep, absoluteExpiration.DateTime, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            return old ;
        }

        /// <summary>
        /// Adds or updates a key with the provided value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="monitor"></param>
        /// <returns></returns>
        public void Set(string key, object value, TimeSpan slidingExpiration, ChangeMonitor monitor = null)
        {
            Add(key, value, slidingExpiration, monitor);
        }

        /// <summary>
        /// Adds or updates a key with the provided value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="monitor"></param>
        public void Set(string key, object value, DateTimeOffset absoluteExpiration, ChangeMonitor monitor = null)
        {
            Add(key, value, absoluteExpiration, monitor);
        }

        public object Remove(string key)
        {
           key.MustNotBeNull();
            return _cache.Remove(key);
        }

        public void Refresh(string key, DateTimeOffset absoluteExpiration)
        {
            Get(key, absoluteExpiration);
        }

        /// <summary>
        /// Returns the underlying cache (the real caching object) used by this adapter.
        ///  </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public T GetUnderlyingCacheAs<T>() where T : class
        {
            var c= _cache as T;
            if (c==null) throw new InvalidCastException("Cache isn't of type '{0}'".ToFormat(typeof(T)));
            return c;
        }
    }
}