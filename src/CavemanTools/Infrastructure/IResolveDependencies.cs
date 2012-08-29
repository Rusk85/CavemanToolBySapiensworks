using System;

namespace CavemanTools.Infrastructure
{
    public interface IResolveDependencies
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}