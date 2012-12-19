using System;
using CavemanTools.Infrastructure.MessagesBus.Internals;
using CavemanTools.Logging;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public static class MessageBusFactoryExtensions
    {
        /// <summary>
        /// Just for testing purposes
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigureMessageBusFactory UseInMemoryPersistence(this IConfigureMessageBusFactory config)
        {
            return config.WithStorageFactory(()=>new InMemoryBusStorage());            
        }

        /// <summary>
        /// Configures the message bus factory to use the default type activator as the dependency resolver.
        /// It is STRONGLY suggested that you use a real DI Container in production.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigureMessageBusFactory UseDefaultTypeActivator(this IConfigureMessageBusFactory config)
        {
            config.WithDependencyResolver(ActivatorContainer.Instance);
            return config;
        }

        /// <summary>
        /// Register the provided type as a handler. The handler must handle at least one message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigureMessageBusFactory UseHandlersFrom<T>(this IConfigureMessageBusFactory config)
        {
            config.UseHandlersFrom(typeof (T));
            return config;
        }
        /// <summary>
        /// The bus will work faster but you'll lose the recovery feature
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigureMessageBusFactory WithNoPersistence(this IConfigureMessageBusFactory config)
        {
            return config.WithStorageFactory(()=>new NullStorage());
        }
        
        /// <summary>
        /// Implementations of <see cref="ILogWriter"/> and <see cref="IStoreMessageBusState"/> must be registered in the container
        /// </summary>
        /// <param name="config"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static IConfigureMessageBusFactory UseResolverToPopulateDependencies(
            this IConfigureMessageBusFactory config, IContainerScope resolver)
        {
            config.WithDependencyResolver(resolver);
            config.WithLogger(resolver.Resolve<ILogWriter>());
            config.WithStorageFactory(resolver.Resolve<IStoreMessageBusState>);
            return config;
        }
    }
}