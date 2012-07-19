using System;

namespace CavemanTools.Infrastructure
{
    public interface ISetupServiceBus
    {
        /// <summary>
        /// Only one handler per command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="DuplicateCommandHandlerException"></exception>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDisposable RegisterCommandHandlerFor<T>(Action<T> handler) where T : ICommand;
        /// <summary>
        /// Only one handler per command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDisposable RegisterCommandHandlerFor<T>(IHandleCommand<T> handler) where T : ICommand;

        /// <summary>
        /// Subscribe to event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        IDisposable SubscribeToEvent<T>(Action<T> subscriber) where T : IEvent;
        IDisposable SubscribeToEvent<T>(IHandleEvent<T> subscriber) where T : IEvent;
        /// <summary>
        /// Subscribe a handler to be notified about the specified error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDisposable SubscribeToError<T>(Action<T> handler) where T : AbstractErrorMessage;

        /// <summary>
        /// Set to false if you want the unhandled exception to be published as error notifications by the bus
        /// Default is true
        /// </summary>
        bool ThrowOnUnhandledExceptions { get; set; }
        /// <summary>
        /// True to ignore if an error message has no subscribers.
        /// Default is false
        /// </summary>
        bool IgnoreLackOfSubscribers { get; set; }

        /// <summary>
        /// Register a handler for a message
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDisposable RegisterHandler(Type msgType,object handler);

        IDisposable SubscribeToError<T>(IHandleError<T> handler) where T : AbstractErrorMessage;
    }
}