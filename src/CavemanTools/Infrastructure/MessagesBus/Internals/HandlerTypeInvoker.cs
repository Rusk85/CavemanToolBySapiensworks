using System;
using CavemanTools.Logging;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    class HandlerTypeInvoker:IInvokeMessageHandler
    {
        private readonly Type _handlerType;
        private readonly IContainerScope _container;
        
        public HandlerTypeInvoker(Type handlerType,IContainerScope container)
        {
            handlerType.MustNotBeNull();
            container.MustNotBeNull();
            _handlerType = handlerType;
            _container = container;
        }

        public void Execute(dynamic msg)
        {
            using (var r = _container.BeginLifetimeScope())
            {
                dynamic h =r.Resolve(_handlerType);
                h.Execute(msg);
            }
        }

        public dynamic ExecuteRequest(dynamic msg)
        {
            using (var r = _container.BeginLifetimeScope())
            {
                dynamic h = r.Resolve(_handlerType);
                return h.Execute(msg);
            }
        }

        public void Publish(dynamic evnt)
        {
            using (var r = GetInstance())
            {
                r.Object.Handle(evnt);
            }
        }

        public IDisposableHandlerInstance GetInstance()
        {
            return new ContainerBasedInstance(_container,_handlerType);
        }

        public Type HandlerType
        {
            get { return _handlerType; }
        }

        class ContainerBasedInstance:IDisposableHandlerInstance
        {
            private readonly IContainerScope _container;
            
            public ContainerBasedInstance(IContainerScope container,Type handlerType)
            {
                LogHelper.DefaultLogger.Debug("[HandlerTypeInvoker] Begining lifetime scope for type {0}",handlerType);
                _container = container.BeginLifetimeScope();
                Object = _container.Resolve(handlerType);
                LogHelper.DefaultLogger.Debug("[HandlerTypeInvoker] Instantiated object of type {0} ",handlerType);
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                _container.Dispose();
                LogHelper.DefaultLogger.Debug("[HandlerTypeInvoker] Ending lifetime scope");
            }

            public dynamic Object { get; private set; }
        }
    }
}