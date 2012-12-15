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
            private object _handler;

            public DisposableSubscriber(EventExecutor parent,object handler)
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

        private List<object> _subs= new List<object>();

        public EventExecutor(Type messageType) : base(messageType)
        {
        }

        internal List<object> Subs
        {
            get { return _subs; }
        }

        public IDisposable AddSubscriber(object handler)
        {
            var sub = new DisposableSubscriber(this, handler);
            return sub;
        }

        public IDisposable AddSagaHandler(object handler,IResolveSagaRepositories resolver,IDispatchCommands bus)
        {
            var saga = new SagaExecutor(handler, resolver,bus);
            return AddSubscriber(saga);
        }

        public override void Handle(IMessage msg)
        {
            foreach(dynamic handler in Subs)
            {
                handler.Handle((dynamic) msg);
            }
        }
    }
}