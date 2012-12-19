using System;
using System.Reflection;
using CavemanTools.Logging;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IConfigureMessageBusFactory
    {
        IConfigureMessageBusFactory WithLogger(ILogWriter log);
        /// <summary>
        /// All unhandled exceptions go there. If nothing is provided, exceptions are only logged.
        /// Exceptions are always logged regardless if you set a handler or not.
        /// </summary>
        /// <param name="errorHandler"></param>
        /// <returns></returns>
        IConfigureMessageBusFactory SetGlobalErrorHandler(Action<Exception> errorHandler);
        IConfigureMessageBusFactory WithDependencyResolver(IContainerScope resolver);
        IConfigureMessageBusFactory WithStorageFactory(Func<IStoreMessageBusState> storage);
        /// <summary>
        /// Autoregister message handlers from the specified assemblies
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        IConfigureMessageBusFactory ScanForHandlers(params Assembly[] assemblies);
        /// <summary>
        /// Specified tyeps contain message handlers
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IConfigureMessageBusFactory UseHandlersFrom(params Type[] types);
        IMessageBusFactory Build();

    }
}