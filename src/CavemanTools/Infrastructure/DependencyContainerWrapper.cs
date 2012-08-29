using System;

namespace CavemanTools.Infrastructure
{
    public class DependencyContainerWrapper:IResolveDependencies
    {
        private readonly Func<Type, object> _resolver;

        public DependencyContainerWrapper(Func<Type,object> resolver)
        {
            _resolver = resolver;
        }

        public object Resolve(Type type)
        {
            return _resolver(type);
        }

        public T Resolve<T>()
        {
            return (T) _resolver(typeof (T));
        }
    }
}