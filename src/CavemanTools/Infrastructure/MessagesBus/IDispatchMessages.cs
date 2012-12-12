using System;
using System.Threading.Tasks;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IDispatchMessages : IDispatchCommands,IDispatchCommandsAsync
    {
        /// <summary>
        /// Publishes asynchronously the events.
        /// All events are published on the same thread
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
   
}