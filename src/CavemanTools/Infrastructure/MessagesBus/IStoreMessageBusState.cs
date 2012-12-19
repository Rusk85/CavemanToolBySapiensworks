using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IStoreMessageBusState
    {
        /// <summary>
        /// Stores and marks a message as in progress
        /// </summary>
        /// <exception cref="DuplicateMessageException"></exception>
        /// <param name="cmd"></param>
        void StoreMessageInProgress(IMessage cmd);
        void MarkMessageCompleted(Guid id);
        void MarkMessageFailed(Guid id);

        /// <summary>
        /// Gets all the un completed messages, usually as aresult of server crash
        /// </summary>
        /// <param name="ignoreFailedCount">Ignore failed messages with a failure count greater than value</param>
        /// <returns></returns>
        IEnumerable<IMessage> GetUncompletedMessages(int ignoreFailedCount);

        /// <summary>
        /// Do message store cleanup
        /// </summary>
        void Cleanup();

        void EnsureStorage();

    }

}