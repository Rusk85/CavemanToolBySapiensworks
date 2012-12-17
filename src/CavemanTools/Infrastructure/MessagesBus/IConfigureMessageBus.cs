using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IConfigureMessageBus
    {
        bool IgnoreMissingCommandHandler { get; set; }
        ///// <summary>
        ///// True if you want that all async commandes to be queued rather than to be executed immediately on a different thread
        ///// Default is false
        ///// </summary>
        //bool QueueAsyncCommands { get; set; }
        ///// <summary>
        ///// Async commands are queued to be executed after the specified delay.
        ///// Used only if async commands are queued.
        ///// Default is 0
        ///// </summary>
        //TimeSpan QueueDelay { get; set; }
        IDisposable RegisterHandler(Type msgType, object handler);
        IDisposable RegisterHandler<T>(ISubscribeToEvent<T> handler) where T:IEvent;
        IDisposable RegisterHandler<T>(IExecuteCommand<T> handler) where T:ICommand;
        IDisposable SetupCommandHandler<T>(Action<T> handler) where T:ICommand;
        IDisposable SetupEventHandler<T>(Action<T> handler) where T:IEvent;
        IDisposable RegisterHandler<T,R>(IExecuteRequest<T,R> handler) where T:ICommand;
        /// <summary>
        /// The handler class must have public visibility
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        IDisposable RegisterHandlerType(Type msgType, Type handlerType);
    }
}