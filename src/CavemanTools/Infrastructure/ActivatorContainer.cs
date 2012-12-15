using System;

namespace CavemanTools.Infrastructure
{
    public class ActivatorContainer:DependencyContainerWrapper
    {
        static IResolveDependencies _inst= new ActivatorContainer();
        public static IResolveDependencies Instance
        {
            get { return _inst; }
        }
        private ActivatorContainer() : base(t=>Activator.CreateInstance(t))
        {
        }
    }
}