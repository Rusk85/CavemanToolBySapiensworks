
using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public abstract class Saga<T>:ISaga<T> where T : ISagaState
    {
        /// <summary>
        /// Gets or sets the saga state
        /// </summary>
        public T Data{ get; set; }

       protected void MarkAsComplete()
        {
            Data.IsCompleted = true;
        }

        internal void SetBus(IDispatchCommands bus)
        {
            bus.MustNotBeNull();
            Bus = bus;
        }

        /// <summary>
        /// Bus for sending commands
        /// </summary>
        protected IDispatchCommands Bus { get; set; }
    }
}