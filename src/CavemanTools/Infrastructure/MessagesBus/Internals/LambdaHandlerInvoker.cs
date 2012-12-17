using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    class LambdaHandlerInvoker<T>:IInvokeMessageHandler where T:IMessage
    {
        private readonly Action<T> _handler;

        public LambdaHandlerInvoker(Action<T> handler)
        {
            _handler = handler;
            HandlerType = typeof (T);
        }

        public void Execute(dynamic msg)
        {
            _handler((T)msg);
        }

        public dynamic ExecuteRequest(dynamic msg)
        {
            throw new NotImplementedException();
        }

        public void Publish(dynamic evnt)
        {
            _handler((T)evnt);
        }

        public IDisposableHandlerInstance GetInstance()
        {
            throw new NotImplementedException();
        }

        public Type HandlerType { get; private set; }
    }
}