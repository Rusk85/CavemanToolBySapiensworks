using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public static class MessageBusFactoryExtensions
    {
        public static IConfigureMessageBusFactory UseInMemoryPersistence(this IConfigureMessageBusFactory config)
        {
            //config.WithStorage(new )
            return config;
        }

        /// <summary>
        /// Configures the message bus factory to use the default type activator as the dependency resolver.
        /// It is STRONGLY suggested that you use a real DI Container in production.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigureMessageBusFactory UseDefaultTypeActivator(this IConfigureMessageBusFactory config)
        {
            config.WithDependencyResolver(new DependencyContainerWrapper(t => Activator.CreateInstance(t)));
            return config;
        }

        public static IConfigureMessageBusFactory UseHandlersFrom<T>(this IConfigureMessageBusFactory config)
        {
            config.UseHandlersFrom(typeof (T));
            return config;
        }
    }
}