using System;
using System.Collections.Generic;
using CavemanTools.Infrastructure.MessagesBus.Saga;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class EventExecutor:MessageHandlerExecutor
    {
        class DisposableSubscriber:IDisposable
        {
            private EventExecutor _parent;
            private IExecuteMessageHandler _handler;

            public DisposableSubscriber(EventExecutor parent,IExecuteMessageHandler handler)
            {
                _parent = parent;
                _parent.Subs.Add(handler);
                _handler = handler;
            }

            public void Dispose()
            {
                if (_parent!=null)
                {
                    _parent.Subs.Remove(_handler);
                    _parent = null;
                    _handler = null;
                }                
            }
        }

        private List<IExecuteMessageHandler> _subs= new List<IExecuteMessageHandler>();

        public EventExecutor(Type messageType) : base(messageType)
        {
        }

        internal List<IExecuteMessageHandler> Subs
        {
            get { return _subs; }
        }

        public IDisposable AddSubscriber(Func<IInvokeMessageHandler> handler)
        {
            var sub = new DisposableSubscriber(this, new SimpleEventExecutor(handler));
            return sub;
        }

        public IDisposable AddSagaHandler(Func<IInvokeMessageHandler> handler,IResolveSagaRepositories resolver,IDispatchCommands bus)
        {
            var saga = new SagaExecutor(handler, resolver,bus);
            return new DisposableSubscriber(this, saga);
        }

        public override void Handle(IMessage msg)
        {
            foreach(var handler in Subs)
            {
                handler.Handle(msg);
            }
        }

        class SimpleEventExecutor:IExecuteMessageHandler
        {
            private readonly Func<IInvokeMessageHandler> _invoker;

            public SimpleEventExecutor(Func<IInvokeMessageHandler> invoker)
            {
                _invoker = invoker;
            }

            public void Handle(IMessage msg)
            {
                _invoker().Publish(msg as IEvent);
            }
        }
    }
}