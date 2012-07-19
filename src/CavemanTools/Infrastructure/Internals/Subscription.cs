using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.Internals
{
    internal class HandlerWrapper
    {
        private dynamic _h;

        public void Wrap<T>(Action<T> handler) where T:IMessage
        {
            _h = handler;
        }
        public void Handle(dynamic msg)
        {
            _h(msg);
        }
    }

    internal class Subscription:IDisposable
        
    {
        private IRemoveHandler _manager;
        private Type _event;
        
         private dynamic _handler;
      

        public Subscription(IRemoveHandler d,Type evnt,object handler)
        {
            if (d == null) throw new ArgumentNullException("d");
          
            if (handler == null) throw new ArgumentNullException("handler");
            _manager = d;
            _event = evnt;
            _handler = handler;
          
        }

        public bool CanHandle(Type evnt)
        {
            return _event.IsAssignableFrom(evnt);
        }

        public bool IsExactlyFor(Type evnt)
        {
            return _event.Equals(evnt);
        }

        public void Handle(IMessage evnt)
        {
            _handler.Handle((dynamic)evnt);            
        }

        public void Dispose()
        {
            _manager.Unsubscribe(this);
        }

      
        public override int GetHashCode()
        {
            unchecked
            {
                return _event.GetHashCode()*29 + _handler.GetHashCode();
            }
            
        }
    }
}