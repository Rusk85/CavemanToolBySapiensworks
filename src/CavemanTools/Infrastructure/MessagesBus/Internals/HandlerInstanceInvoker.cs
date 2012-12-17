using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    class HandlerInstanceInvoker:IInvokeMessageHandler
    {
        private readonly dynamic _handler;

        public HandlerInstanceInvoker(object handler)
        {
            _handler = handler;
            HandlerType = handler.GetType();
        }


        public void Execute(dynamic msg)
        {
            _handler.Execute(msg);
        }

        public dynamic ExecuteRequest(dynamic msg)
        {
            return _handler.Execute(msg);
        }

        public void Publish(dynamic evnt)
        {
            _handler.Handle(evnt);
        }

        public IDisposableHandlerInstance GetInstance()
        {
            return new DisposableHandlerInstance(_handler);
        }

        public Type HandlerType
        {
            get; private set;
        }

        class DisposableHandlerInstance : IDisposableHandlerInstance
        {
            public DisposableHandlerInstance(object handler)
            {
                Object = handler;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                
            }

            public dynamic Object { get; private set; }
        }
    }
}