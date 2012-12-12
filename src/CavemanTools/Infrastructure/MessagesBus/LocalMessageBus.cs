using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CavemanTools.Infrastructure.MessagesBus.Internals;
using CavemanTools.Infrastructure.MessagesBus.Saga;
using CavemanTools.Logging;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public class LocalMessageBus : IMessageBus
    {
        private IStoreMessageBusState _storage;
        private readonly ILogWriter _log;
        private readonly IResolveSagaRepositories _sagaResolver;

        public LocalMessageBus(IStoreMessageBusState storage, ILogWriter log, IResolveSagaRepositories sagaResolver)
        {
            storage.MustNotBeNull("storage");
            log.MustNotBeNull("log");
            sagaResolver.MustNotBeNull("sagaResolver");        
            _storage = storage;
            _log = log;
            _sagaResolver = sagaResolver;        
            ThrowOnUnhandledExceptions = true;
            IgnoreMissingCommandHandler = false;
            RecoveryMode = false;            
        }

        internal bool RecoveryMode { get; set; }

       SubscriptionManager _subs= new SubscriptionManager();
        

        private bool ThrowOnUnhandledExceptions
        { get; set; }

     
        internal SubscriptionManager Subscriptions
        {
            get { return _subs; }
        }

        #region Handler from instance registration

        IDisposable RegisterCommands(Type[] interfaces,object handler,Type msgType)
        {
            MessageHandlerExecutor exec = null;
            if (interfaces.CanExecuteRequest())
            {
                exec = new RequestExecutor(msgType, handler);
            }
            else
            {
                if (interfaces.CanExecuteCommand())
                {
                    exec = new CommandExecutor(msgType, handler);
                }
            }
            
            
            if (exec != null)
            {
                return _subs.Add(exec);
            }
            return null;
        }

       IDisposable RegisterEvents(Type[] interfaces, object handler, Type msgType)
        {
            var sub = _subs.GetOrCreate(msgType);
            if (interfaces.CanStartSaga())
            {
                return sub.AddSagaHandler(handler, _sagaResolver);
            } 

           if (interfaces.CanHandleEvent())
            {
                return sub.AddSubscriber(handler);
            }
                        
            return null;
        }

      

        public IDisposable RegisterHandler(Type msgType, object handler)
        {
            if (!msgType.Implements<IMessage>()) throw new ArgumentException("Message must implement IMessage");
            handler.MustNotBeNull();
            var tp = handler.GetType();

            var interfaces = MessageHandlerDiscoverer.GetImplementedInterfaces(tp, msgType).ToArray();
         
            if (interfaces.Length == 0) throw new ArgumentException("The object provided doesn't contain valid messages handlers");

            IDisposable result = null;

            if (msgType.Implements<ICommand>())
            {
                result=RegisterCommands(interfaces, handler, msgType);
            }
            else
            {
                if (msgType.Implements<IEvent>())
                {
                    result = RegisterEvents(interfaces, handler, msgType);
                }
            }
            
            if (result == null) throw new ArgumentException("Message type is not an ICommand or IEvent");
            _log.Info("Registered '{0}' as handler for '{1}'", handler, msgType);
            return result;
        } 
        #endregion

        public IDisposable RegisterHandler<T>(ISubscribeToEvent<T> handler) where T:IEvent
        {
            _log.Debug("Tyring to register handler '{0}' for event '{1}'",handler.ToString(),typeof(T));
            var exec = _subs.GetOrCreate(typeof (T));
            var res= exec.AddSubscriber(handler);
            _log.Info("Handler '{0}' was registered for event '{1}'", handler.ToString(), typeof(T));
            return res;
        }

        public IDisposable RegisterHandler<T>(IExecuteCommand<T> handler) where T:ICommand
        {
            _log.Debug("Trying to register handler '{0}' for command '{1}'", handler.ToString(), typeof(T));
            var exec = new CommandExecutor(typeof(T),handler);
            var res = AddCommandHandler(exec);
            _log.Info("Handler '{0}' was registered for command '{1}'", handler.ToString(), typeof(T));
            return res;
        }

        public IDisposable RegisterHandler<T,R>(IExecuteRequest<T,R> handler) where T:ICommand
        {
            _log.Debug("Trying to register handler '{0}' for command '{1}'", handler.ToString(), typeof(T));
            var exec = new RequestExecutor(typeof(T), handler);
            var res = AddCommandHandler(exec);
            _log.Info("Handler '{0}' was registered for command '{1}'", handler.ToString(), typeof(T));
            return res;
        }


        IDisposable AddCommandHandler(MessageHandlerExecutor executor)
        {
            try
            {
                IDisposable res = _subs.Add(executor);
                return res;
            }
            catch(DuplicateCommandHandlerException)
            {
                _log.Error("Command '{0}' already has a handler!",executor.MessageType);
                throw;
            }

        }

        public IDisposable SetupCommandHandler<T>(Action<T> handler) where T:ICommand
        {
            _log.Debug("Trying lambda handler for command '{0}'", typeof(T));
            var exec = new HandlerWrapper(typeof (T));
            exec.Wrap(handler);
            var res = AddCommandHandler(exec);
            _log.Info("Lambda handler registered for command '{0}'", typeof(T));
            return res;
        }
        
        public IDisposable SetupEventHandler<T>(Action<T> handler) where T:IEvent
        {
            _log.Debug("Trying to register lambda handler for event '{0}'", typeof(T));
            var sub = _subs.GetOrCreate(typeof (T));
            var exec = new HandlerWrapper(typeof (T));
            exec.Wrap(handler);
            var res= sub.AddSubscriber(exec);
            _log.Info("Lambda handler registered for event '{0}'", typeof(T));
            return res;
        }

        public bool IgnoreMissingCommandHandler { get; set; }

      

        #region Request
        public R Request<R>(ICommand cmd)
        {
            return ExecuteRequest(cmd);
        }

        dynamic ExecuteRequest(ICommand cmd)
        {
            dynamic result=null;
            var handler = GetHandlerForCommand(cmd);
            if (handler == null)
            {
                if (!IgnoreMissingCommandHandler) throw new MissingCommandHandlerException("Command has no handler set");                
            }
            
            if (!RecoveryMode)
            {
                try
                {
                    _log.Debug("Command '{0}' persisted as in progress", cmd.GetType());
                    _storage.StoreMessageInProgress(cmd);
                }
                catch (DuplicateMessageException)
                {
                    _log.Debug("Command '{0}' already in progress. Ignoring.", cmd.GetType());
                    throw;
                } 
            }          

            try
            {
                _log.Debug("Executing handler for command '{0}'", cmd.GetType());
                result=handler.ExecuteRequest(cmd);
                _storage.StoreMessageCompleted(cmd.Id);
                _log.Debug("Command '{0}' completed", cmd.GetType());
            }
            catch (Exception ex)
            {
                _log.Error("Handling command {0} threw exception: {1}", cmd, ex);
                if (ThrowOnUnhandledExceptions) throw;
                else
                {
                    var err = new UnhandledMessageException(ex);
                    Notify(err);
                }
            }
            return result;
        }

        #endregion

        #region Commands
        public void Send(ICommand cmd)
        {
            _log.Debug("Sending command '{0}'", cmd.GetType());
            ExecuteCommand(cmd);
        }

        public void Send<T>(Action<T> constructor) where T : ICommand,new()
        {
            T cmd= new T();
            constructor(cmd);
            Send(cmd);
        }

        public Task SendAsync(ICommand cmd)
        {
           _log.Debug("Sending async command '{0}'", cmd.GetType());
            return Task.Factory.StartNew(() => ExecuteCommand(cmd));
        }

        public Task SendAsync<T>(Action<T> constructor) where T : ICommand,new()
        {
            T cmd = new T();
            constructor(cmd);
            return SendAsync(cmd);
        }

      

        #region ExecuteCommand
        MessageHandlerExecutor GetHandlerForCommand(ICommand cmd)
        {
            return _subs.Get(cmd.GetType());
        }

      
        void ExecuteCommand(ICommand cmd)
        {
          
            var handler = GetHandlerForCommand(cmd);
            if (handler == null)
            {
                if (!IgnoreMissingCommandHandler) throw new MissingCommandHandlerException("Command has no handler set");
                return;
            }
           
            if (!RecoveryMode)
            {
                try
                {
                    _log.Debug("Command '{0}' persisted as in progress", cmd.GetType());
                    _storage.StoreMessageInProgress(cmd);
                }
                catch (DuplicateMessageException)
                {
                    _log.Debug("Command '{0}' already in progress. Ignoring.", cmd.GetType());
                    return;
                }
 
            }
         
            try
            {
                _log.Debug("Executing handler for command '{0}'", cmd.GetType());
                handler.Handle(cmd);
                _storage.StoreMessageCompleted(cmd.Id);
                _log.Debug("Command '{0}' completed", cmd.GetType());
            }
            catch (Exception ex)
            {
                _log.Error("Handling command {0} threw exception: {1}",cmd,ex);
                if (ThrowOnUnhandledExceptions) throw;
                else
                {
                    var err = new UnhandledMessageException(ex);
                    Notify(err);
                }
            }

        }
        #endregion

        #endregion

        #region Events

        public void Publish(params  IEvent[] events)
        {
            PublishEvents(StoreEvents(events));
        }


        /// <summary>
        /// Publishes asynchronously the events
        /// </summary>
        /// <param name="events"></param>
        public Task PublishAsync(params IEvent[] events)
        {
            var exec = StoreEvents(events);

            return Task.Factory.StartNew(evs => PublishEvents(evs),exec);
        }

        private void PublishEvents(object evs)
        {
            List<Exception> exceptions = new List<Exception>();

            var allExceptions = new AggregateException();

            foreach (var ev in (evs as IEvent[]))
            {
                try
                {
                    _log.Debug("Executing handler for event '{0}'", ev);
                    var h = _subs.Get(ev.GetType());
                    h.Handle(ev);
                    _storage.StoreMessageCompleted(ev.Id);
                    _log.Debug("Publishing of event '{0}' completed", ev);
                }
                catch (Exception ex)
                {
                    _log.Error("Handling event '{0}' threw exeption\n{1}", ev, ex.ToString());
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        private IEvent[] StoreEvents(IEvent[] events)
        {
            if (RecoveryMode) return events;
            var exec = events.Select(ev =>
                                         {
                                             if (ev.SourceId == null)
                                                 throw new ArgumentException(
                                                     "Every message must have set the SourceId property. An event should have as its SourceId the command id of command which generated the event.");
                                             try
                                             {
                                                 _log.Debug("Event '{0}' persisted as in progress", ev);
                                                 _storage.StoreMessageInProgress(ev);
                                                 return ev;
                                             }
                                             catch (DuplicateMessageException)
                                             {
                                                 _log.Debug("Event '{0}' already in progress. Ignoring.", ev.GetType());
                                                 return null;
                                             }
                                         }).Where(e => e != null).ToArray();
            return exec;
        }

        #endregion

        #region Errors

        private void Notify(AbstractErrorMessage ex)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            //   SendMessage(ex);
        } 
        #endregion

     
    }

    

   
}