using System;
using System.Collections.Generic;
using System.Linq;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    class SubscriptionManager:IDisposable
    {
        class DisposableSubscription:IDisposable
        {
            private SubscriptionManager _manager;
            private readonly Type _t;

            public DisposableSubscription(SubscriptionManager manager,Type t)
            {
                _manager = manager;
                _t = t;
            }
            
            public void Dispose()
            {
                if (_manager!=null)
                {
                    _manager._list.Remove(_t);
                    _manager = null;
                }
               
            }
        }
        Dictionary<Type,MessageHandlerExecutor> _list=new Dictionary<Type, MessageHandlerExecutor>();
       
        public MessageHandlerExecutor Get(Type t)
        {
            MessageHandlerExecutor res;
            if (_list.TryGetValue(t,out res))
            {
                return res;
            }
            return null;
        }

        /// <summary>
        /// Creates one new if not found
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public EventExecutor GetEventExecutor(Type t)
        {
            var e = Get(t);
            if (e==null)
            {
                e= new EventExecutor(t);
                _list.Add(t,e);
            }
            return e as EventExecutor;
        }

        public IDisposable Add(MessageHandlerExecutor t)
        {
            var sub = new DisposableSubscription(this, t.MessageType);
            try
            {
                _list.Add(t.MessageType,t);
                return sub;
            }
            catch (ArgumentException)
            {
                throw new DuplicateCommandHandlerException();
            }   
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
           if (_list != null)
           {
               _list.Values.ForEach(v=>v.Dispose());
               _list.Clear();
               _list = null;
           }
           
        }
    }
}