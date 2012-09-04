using System;
using CavemanTools.Infrastructure.MessagesBus;

namespace CavemanTools.Infrastructure
{
    public interface IQueueCommands
    {
        /// <summary>
        /// Queue the command to be executed asap
        /// </summary>
        /// <param name="cmd"></param>
        void Queue(ICommand cmd);
        /// <summary>
        /// Queue the command to be executed after the specified interval
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="afterInterval"></param>
        void Queue(ICommand cmd,TimeSpan afterInterval);
        /// <summary>
        /// Queue item to be executed at the specified UTC time
        /// </summary>
        /// <param name="command"></param>
        /// <param name="when">UTC date</param>
        void Queue(ICommand command,DateTime when);
    }
}