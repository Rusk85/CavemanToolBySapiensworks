using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class NullStorage:IStoreMessageBusState
    {
        public void StoreMessageInProgress(IMessage cmd)
        {
            
        }

        public void MarkMessageCompleted(Guid id)
        {
            
        }

        public void MarkMessageFailed(Guid id)
        {
            
        }

        public IEnumerable<IMessage> GetUncompletedMessages(int f)
        {
            return new IMessage[0];
        }

        public void Cleanup()
        {
            
        }

        public void EnsureStorage()
        {
            
        }
    }
}