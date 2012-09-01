using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class NullStorage:IStoreMessageBusState
    {
        public void StoreMessageInProgress(IMessage cmd)
        {
            
        }

        public void StoreMessageCompleted(Guid id)
        {
            
        }

        public IEnumerable<IMessage> GetUncompletedMessages()
        {
            return new IMessage[0];
        }

        public void Cleanup()
        {
            
        }
    }
}