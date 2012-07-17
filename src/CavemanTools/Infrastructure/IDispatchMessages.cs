using CavemanTools.Infrastructure.Internals;

namespace CavemanTools.Infrastructure
{
    public interface IDispatchMessages
    {
        /// <summary>
        /// Sends command to be handled (oby one handler only)
        /// </summary>
        /// <param name="cmd"></param>
        void Send(ICommand cmd);
        
        /// <summary>
        /// Publish event to its subscribers
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="raise"></param>
        void Publish(IEvent evnt,HandlingType raise=HandlingType.Specific);

        /// <summary>
        /// Notifies subscribers about an error.
        /// </summary>
        /// <param name="ex"></param>
        /// <exception cref="NoSubscribersException"></exception>
        void Notify(AbstractErrorMessage ex);
        /// <summary>
        /// Starts buffering any message received
        /// </summary>
        /// <returns></returns>
        IBufferMessages BeginBuffering();
        /// <summary>
        /// Sends all the buffered messages and ends the buffering
        /// </summary>
        void FlushBuffer();
        bool IsBuffering { get; }
        /// <summary>
        /// Clear all the buffered messages and ends the buffering
        /// </summary>
        void ClearBuffer();
    }
}