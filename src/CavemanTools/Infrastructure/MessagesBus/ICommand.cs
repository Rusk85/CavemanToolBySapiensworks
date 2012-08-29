using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface ICommand:IMessage
    {
        /// <summary>
        /// Enrol the event in the current operation.
        /// An event is enroled in an operation when it contains the operation id
        /// </summary>
        /// <param name="evnt"></param>
        void Enrol(IEvent evnt);
        /// <summary>
        /// Creates an event enroled in the current operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <returns></returns>
        T CreateEvent<T>(Action<T> constructor) where T : IEvent,new();
    }
}