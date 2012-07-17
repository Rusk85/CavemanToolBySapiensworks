using System;
using System.Collections.Generic;
using System.Linq;
using CavemanTools.Infrastructure.Internals;
using Buffer = CavemanTools.Infrastructure.Internals.Buffer;

namespace CavemanTools.Infrastructure
{
    public class ServiceBus:IBus,IRemoveHandler
    {
        public ServiceBus()
        {
            ThrowOnUnhandledExceptions = true;
        }

        internal ServiceBus(ServiceBus sb)
        {
           _subs.AddRange(sb._subs);
        }

        private List<ISubscription> _subs= new List<ISubscription>();
        public IDisposable RegisterCommandHandlerFor<T>(Action<T> handler) where T : ICommand
        {
            if (handler == null) throw new ArgumentNullException("handler");
            if (ExistsCommandHandler(typeof(T)))
           {
               throw new DuplicateCommandHandlerException();
           }
            return AddSubscription(handler);
        }

        IDisposable AddSubscription<T>(Action<T> handler) where T:IMessage
        {
            var s = new Subscription<T>(this, typeof(T), handler);
            lock (_sync)
            {
                _subs.Add(s);
            }
            return s;
        }

        bool ExistsCommandHandler(Type cmd)
        {
            return _subs.Any(s => s.IsExactlyFor(cmd));
        }

        public IDisposable RegisterCommandHandlerFor<T>(IHandleCommand<T> handler) where T : ICommand
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return RegisterCommandHandlerFor<T>(handler.Handle);
        }

        public IDisposable SubscribeToEvent<T>(Action<T> subscriber) where T : IEvent
        {
            if (subscriber == null) throw new ArgumentNullException("subscriber");
            return AddSubscription(subscriber);
        }

        public IDisposable SubscribeToEvent<T>(IHandleEvent<T> subscriber) where T : IEvent
        {
            if (subscriber == null) throw new ArgumentNullException("subscriber");
            return SubscribeToEvent<T>(subscriber.Handle);
        }

        public IDisposable SubscribeToError<T>(Action<T> handler) where T : AbstractErrorMessage
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return AddSubscription(handler);
        }

        public bool ThrowOnUnhandledExceptions
        { get ; set; }

        public bool IgnoreLackOfSubscribers
        {
            get;
            set;
        }

        public void Send(ICommand cmd)
        {
            SendMessage(cmd);
        }

        public void Publish(IEvent evnt, HandlingType raise = HandlingType.Specific)
        {
            SendMessage(evnt,raise);
        }

        void SendMessage(IMessage msg,IEnumerable<ISubscription> subs)
        {
          
                foreach (var sub in subs)
                {
                    try
                    {
                        sub.Handle(msg);
                    }
                    catch (Exception ex)
                    {
                        if (ThrowOnUnhandledExceptions) throw;
                        else
                        {
                            var err = new UnhandledMessageException(ex);
                            Notify(err);
                        }
                    }
                }
           
        }

        internal void SendMessage(IMessage msg,HandlingType raise=HandlingType.Specific,bool ignoreBuffer=false)
        {
            if (IsBuffering)
            {
                if (!ignoreBuffer)
                {
                    _buffer.Add(msg,raise);
                    return;
                }
                
            }
            var tp = msg.GetType();
            IEnumerable<ISubscription> subs;
            lock (_sync)
            {
                if (raise == HandlingType.Specific)
                {
                    subs = _subs.Where(s => s.IsExactlyFor(tp)).ToArray();
                }
                else
                {
                    subs = _subs.Where(s => s.CanHandle(tp)).OrderBy(s => s.IsExactlyFor(tp) ? 0 : 1).ToArray();
                }    
            }
            

            if (msg is AbstractErrorMessage)
            {
                if (subs.Count()==0)
                {
                    if (!IgnoreLackOfSubscribers)
                    throw new NoSubscribersException((AbstractErrorMessage)msg);
                }
             }
            SendMessage(msg, subs);
            
        }
        

        public void Notify(AbstractErrorMessage ex)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            SendMessage(ex);
        }

        public IBus SpawnLocal()
        {
            return new ServiceBus(this);
        }

        private Buffer _buffer;
        public IBufferMessages BeginBuffering()
        {
            lock (_sync)
            {
                if (_buffer == null)
                {
                    _buffer = new Buffer(this);
                }
                return _buffer;
            }
        }

        

        private object _sync = new object();
        

        public void FlushBuffer()
        {
            lock (_sync)
            {
                if (_buffer == null) throw new InvalidOperationException("Buffering not started");
                _buffer.Publish();
                _buffer = null;
            }            
        }

        public bool IsBuffering
        {
            get { return _buffer != null; }
        }

        public void ClearBuffer()
        {
            lock (_sync)
            {
               // if (_buffer == null) throw new InvalidOperationException("Buffering not started");
                _buffer = null;
            }
        }

        void IRemoveHandler.Unsubscribe(ISubscription s)
        {
            lock (_sync)
            {
                _subs.Remove(s);
            }
            
        }
    }
}