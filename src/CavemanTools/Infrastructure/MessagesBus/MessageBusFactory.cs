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
        private IResolveDependencies _resolver;

        public void PerformRecovery()
        {
            var all = _storage.GetUncompletedMessages().ToArray();
            if (all.Length==0)
            {
                return;
            }
            var bus = CreateBus() as LocalMessageBus;
            bus.RecoveryMode = true;
            foreach(var ev in all.CastSilently<IEvent>())
            {
                bus.Publish(ev);
            }

            foreach(var cmd in all.CastSilently<ICommand>())
            {
                bus.Send(cmd);
            }
        }

        public IMessageBus CreateBus()
        {
            var bus = new LocalMessageBus(_storage, _logger,new SagaDependecyResolver(_resolver));
            foreach(var handlerType in _handlers)
            {
                var instance = _resolver.Resolve(handlerType.HandlerTypeName);
                foreach(var message in handlerType.MessagesTypes)
                {
                    bus.RegisterHandler(message, instance);
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

        public IConfigureMessageBusFactory WithDependencyResolver(IResolveDependencies resolver)
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
            if (_logger == null || _resolver == null || _storage == null)
                throw new InvalidOperationException("Can't build factory because some elements weren't configured");

            return this;
        }
    }
}