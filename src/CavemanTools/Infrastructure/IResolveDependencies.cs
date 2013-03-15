using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure
{
    public interface IResolveDependencies
    {
        object Resolve(Type type);
        T Resolve<T>();
        /// <summary>
        /// If type is not registered return null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object ResolveOptional(Type type);

        IEnumerable<T> GetServices<T>();
    }
}