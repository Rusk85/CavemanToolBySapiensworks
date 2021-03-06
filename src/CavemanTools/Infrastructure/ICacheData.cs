﻿using System;
using System.Runtime.Caching;

namespace CavemanTools.Infrastructure
{
    public interface IIGetCacheItem
    {
        bool Contains(string key);
        object Get(string key,DateTimeOffset? newExpiration=null);

        /// <summary>
        /// Gets typed object from cache or the supplied default value if the value doesn't exist.
        /// Optionally set a new expiration date.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue">Value to return if the object doesn't exist in cache</param>
        /// <returns></returns>
        T Get<T>(string key,T defaultValue=default(T),DateTimeOffset? newExpiration=null);
    }

    public interface ICacheData : IIGetCacheItem
    {
        /// <summary>
        /// Tries to add value to cache.
        /// Returns the existing value if any
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="monitor"></param>
        /// <returns></returns>
        object Add(string key, object value, TimeSpan slidingExpiration, ChangeMonitor monitor = null);

        /// <summary>
        /// Tries to add value to cache.
        /// Returns the existing value if any
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="monitor"></param>
        object Add(string key, object value, DateTimeOffset absoluteExpiration, ChangeMonitor monitor = null);

        /// <summary>
        /// Adds or updates a key with the provided value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="monitor"></param>
        /// <returns></returns>
        void Set(string key, object value, TimeSpan slidingExpiration, ChangeMonitor monitor = null);

        /// <summary>
        /// Adds or updates a key with the provided value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="monitor"></param>
        void Set(string key, object value, DateTimeOffset absoluteExpiration, ChangeMonitor monitor = null);

        
        object Remove(string key);

        /// <summary>
        /// Sets a new expiration date for the item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="absoluteExpiration"></param>
        void Refresh(string key, DateTimeOffset absoluteExpiration);

        /// <summary>
        /// Returns the underlying cache (the real caching object) used by this adapter.
        ///  </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        T GetUnderlyingCacheAs<T>() where T : class;
    }
}