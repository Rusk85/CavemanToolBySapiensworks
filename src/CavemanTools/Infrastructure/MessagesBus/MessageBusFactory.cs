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
        private IStoreMessageBusState _storage;
        private IContainerScope _resolver;

        public void PerformRecovery()
        {
            var all = _storage.GetUncompletedMessages().ToArray();
            if (all.Length==0)
            {
                return;
            }
            var bus = CreateBus() as LocalMessageBus;
            bus.RecoveryMode = true;
            foreach(var ev in all.CastSilentlyTo<IEvent>())
            {
                bus.Publish(ev);
            }

            foreach(var cmd in all.CastSilentlyTo<ICommand>())
            {
                bus.Send(cmd);
            }
        }

        public IMessageBus CreateBus()
        {
            var bus = new LocalMessageBus(_storage, _resolver,new SagaDependecyResolver(_resolver), _logger);
            foreach(var handlerType in _handlers)
            {
                foreach(var message in handlerType.MessagesTypes)
                {
                    bus.RegisterHandlerType(message, handlerType.HandlerTypeName);
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

        public IConfigureMessageBusFactory WithDependencyResolver(IContainerScope resolver)
        {
            resolver.MustNotBeNull();
            _resolver = resolver;
            return this;
        }

        public IConfigureMessageBusFactory WithStorage(IStoreMessageBusState storage)
        {
            storage.MustNotBeNull();
            _storage = storage;
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
            if (_storage == null)
                throw new InvalidOperationException("Can't build factory without a storage. Use the 'WithNoPersistence' method when configuring the bus");

            return this;
        }
    }
}