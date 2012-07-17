using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.Internals
{
    internal class Subscription<T>:IDisposable,ISubscription where T:IMessage
    {
        private IRemoveHandler _manager;
        private Type _event;
        private Action<T> _handler;

        public Subscription(IRemoveHandler d,Type evnt,Action<T> handler)
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
            _handler((T)evnt);
        }

        public void Dispose()
        {
            _manager.Unsubscribe(this);
        }

        //public bool Equals(Subscription other)
        //{
        //    if (other == null) return false;
        //    return other._event.Equals(_event) && other._handler == _handler;
        //}

        //public override bool Equals(object obj)
        //{
        //    return Equals((Subscription)obj);
        //}

        public override int GetHashCode()
        {
            unchecked
            {
                return _event.GetHashCode()*29 + _handler.GetHashCode();
            }
            
        }
    }
}