﻿using System;

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
    }

    public interface IContainerScope:IResolveDependencies,IDisposable
    {
        IContainerScope BeginLifetimeScope();
    }
}