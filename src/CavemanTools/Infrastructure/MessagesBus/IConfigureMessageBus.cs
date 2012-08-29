using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IConfigureMessageBus
    {
        //bool ThrowOnUnhandledExceptions { get; set; }
        bool IgnoreMissingCommandHandler { get; set; }
        IDisposable RegisterHandler(Type msgType,object handler);
        IDisposable RegisterHandler<T>(IHandleEvent<T> handler) where T:IEvent;
        IDisposable RegisterHandler<T>(IExecuteCommand<T> handler) where T:ICommand;
        IDisposable SetupCommandHandler<T>(Action<T> handler) where T:ICommand;
        IDisposable SetupEventHandler<T>(Action<T> handler) where T:IEvent;
        IDisposable RegisterHandler<T,R>(IExecuteRequest<T,R> handler) where T:ICommand;
    }
}