using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IEvent:IMessage
    {
        /// <summary>
        /// Creates a command as an outcome of the event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <returns></returns>
        T CreateCommand<T>(Action<T> constructor) where T : ICommand, new();
    }
}