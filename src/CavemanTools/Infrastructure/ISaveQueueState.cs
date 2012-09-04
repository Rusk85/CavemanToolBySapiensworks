using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure
{
    public interface ISaveQueueState
    {
        void Save(QueueItem item);
        void ItemWasExecuted(Guid id);
        /// <summary>
        /// Gets items with execution date older than date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<QueueItem> GetItems(DateTime date);
    }
}