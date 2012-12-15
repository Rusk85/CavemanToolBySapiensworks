using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure
{
    public interface ISaveQueueState
    {
        void Save(QueueItem item);
        void MarkItemAsExecuted(Guid id);

        /// <summary>
        /// Gets items with execution date older than date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="maxItems"></param>
        /// <returns></returns>
        IEnumerable<QueueItem> GetItems(DateTime date, int maxItems);
    }
}