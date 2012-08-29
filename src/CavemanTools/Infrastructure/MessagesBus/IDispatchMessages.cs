using System;
using System.Threading.Tasks;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IDispatchMessages
    {
        void Send(ICommand cmd);
        void Send<T>(Action<T> constructor) where T:ICommand,new();
        
        Task SendAsync(ICommand cmd);
        Task SendAsync<T>(Action<T> constructor) where T : ICommand,new();

        /// <summary>
        /// Publishes asynchronously the events
        /// </summary>
        /// <param name="events"></param>
        Task PublishAsync(params IEvent[] events);
        /// <summary>
        /// Executes a command and returns a result
        /// </summary>
        /// <typeparam name="R">Result type</typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        R Request<R>(ICommand cmd);

        void Publish(params  IEvent[] events);
    }

    //public interface IDispatchMessagesOld
    //{
    //    /// <summary>
    //    /// Sends command to be handled (oby one handler only)
    //    /// </summary>
    //    /// <param name="cmd"></param>
    //    void Send(ICommand cmd);
        
    //    /// <summary>
    //    /// Publish event to its subscribers
    //    /// </summary>
    //    /// <param name="evnt"></param>
    //    /// <param name="raise"></param>
    //    void Publish(IEvent evnt,HandlingType raise=HandlingType.Specific);

    //    /// <summary>
    //    /// Notifies subscribers about an error.
    //    /// </summary>
    //    /// <param name="ex"></param>
    //    /// <exception cref="MissingCommandHandlerException"></exception>
    //    void Notify(AbstractErrorMessage ex);
    //    /// <summary>
    //    /// Starts buffering any message received
    //    /// </summary>
    //    /// <returns></returns>
    //    IBufferMessages BeginBuffering();
    //    /// <summary>
    //    /// Sends all the buffered messages and ends the buffering
    //    /// </summary>
    //    void FlushBuffer();
    //    bool IsBuffering { get; }
    //    /// <summary>
    //    /// Clear all the buffered messages and ends the buffering
    //    /// </summary>
    //    void ClearBuffer();
    //}
}