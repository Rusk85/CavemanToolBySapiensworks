using System;

namespace CavemanTools.Infrastructure
{
    public interface IResolveDependencies
    {
        object Resolve(Type type);
        T Resolve<T>();
    }

    public interface IContainerScope:IResolveDependencies,IDisposable
    {
        IContainerScope BeginLifetimeScope();
    }
}