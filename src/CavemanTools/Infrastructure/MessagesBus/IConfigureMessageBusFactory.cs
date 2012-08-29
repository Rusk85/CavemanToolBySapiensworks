using System;
using System.Reflection;
using CavemanTools.Logging;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IConfigureMessageBusFactory
    {
        IConfigureMessageBusFactory WithLogger(ILogWriter log);
        IConfigureMessageBusFactory WithDependencyResolver(IResolveDependencies resolver);
        IConfigureMessageBusFactory WithStorage(IStoreMessageBusState storage);
        IConfigureMessageBusFactory ScanForHandlers(params Assembly[] assemblies);
        IConfigureMessageBusFactory UseHandlersFrom(params Type[] types);
        IMessageBusFactory Build();

    }
}