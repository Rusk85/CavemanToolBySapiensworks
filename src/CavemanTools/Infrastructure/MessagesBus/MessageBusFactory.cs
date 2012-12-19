using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CavemanTools.Infrastructure.MessagesBus.Internals;
using CavemanTools.Infrastructure.MessagesBus.Saga;
using CavemanTools.Logging;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public class MessageBusFactory:IMessageBusFactory,IConfigureMessageBusFactory
    {
        private MessageBusFactory()
        {
            
        }
        public static IConfigureMessageBusFactory Configure()
        {
            return new MessageBusFactory();
        }

        private ILogWriter _logger= NullLogger.Instance;
        private Func<IStoreMessageBusState> _storageFactory;
        private IContainerScope _resolver;

        private int FailureCountToIgnore = 3;

        public void PerformRecovery()
        {
            var all = _storageFactory().GetUncompletedMessages(FailureCountToIgnore).ToArray();
            if (all.Length==0)
            {
                return;
            }
            var bus = CreateBus() as LocalMessageBus;
            bus.RecoveryMode = true;
            _logger.Info("[Message Bus] Starting recovery...");
            _logger.Info("[Message Bus] Publishing unfinished events");
            foreach(var ev in all.CastSilentlyTo<IEvent>())
            {
                bus.Publish(ev);
            }
            _logger.Info("[Message Bus] Executing unfinished commands");
            foreach(var cmd in all.CastSilentlyTo<ICommand>())
            {
                bus.Send(cmd);
            }
            _logger.Info("[Message Bus] Completed recovery");
        }

        private Action<Exception> _onError;

        public IMessageBus CreateBus()
        {
            var bus = new LocalMessageBus(_storageFactory(), _resolver,new SagaDependecyResolver(_resolver), _logger);
            foreach(var handlerType in _handlers)
            {
                foreach(var message in handlerType.MessagesTypes)
                {
                    bus.RegisterHandlerType(message, handlerType.HandlerTypeName);
                    if (_onError != null) bus.OnUnhandledException = _onError;
                }
            }
            return bus;
        }

        public IConfigureMessageBusFactory WithLogger(ILogWriter log)
        {
            log.MustNotBeNull();
            _logger = log;
            return this;
        }

        public IConfigureMessageBusFactory SetGlobalErrorHandler(Action<Exception> errorHandler)
        {
            errorHandler.MustNotBeNull();
            _onError = errorHandler;
            return this;
        }

        public IConfigureMessageBusFactory WithDependencyResolver(IContainerScope resolver)
        {
            resolver.MustNotBeNull();
            _resolver = resolver;
            return this;
        }

        public IConfigureMessageBusFactory WithStorageFactory(Func<IStoreMessageBusState> storage)
        {
            storage.MustNotBeNull();
            _storageFactory = storage;
            return this;
        }

        private List<TypeWithHandlers> _handlers = new List<TypeWithHandlers>();
       
        public IConfigureMessageBusFactory ScanForHandlers(params Assembly[] assemblies)
        {
            foreach(var asm in assemblies)
            {
                UseHandlersFrom(asm.GetExportedTypes());                
            }

            return this;
        }

        public IConfigureMessageBusFactory UseHandlersFrom(params Type[] types)
        {
            var h = types
                   .Select(t => TypeWithHandlers.TryCreateFrom(t))
                   .Where(d => d != null).ToArray();
            _handlers.AddRange(h);
            return this;
        }


        public IMessageBusFactory Build()
        {
            if (_resolver == null)
                throw new InvalidOperationException("Can't build factory without a dependency resolver. If you don't use a DI Container (really?! why? I suggest Autofac) call the 'UseDefaultTypeActivator' method when configuring the bus");
            if (_storageFactory == null)
                throw new InvalidOperationException("Can't build factory without a storage. Use the 'WithNoPersistence' method when configuring the bus");
            _storageFactory().EnsureStorage();
            return this;
        }
    }
}