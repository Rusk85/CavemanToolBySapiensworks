using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure
{
    public interface ISaveQueueState
    {
        void Save(QueueItem item);
        void MarkItemAsExecuted(Guid id);
        void MarkItemAsFailed(Guid id);

        /// <summary>
        /// Gets items with execution date older than date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="maxItems"></param>
        /// <returns></returns>
        IEnumerable<QueueItem> GetItems(DateTime date, int maxItems);
        /// <summary>
        /// Items which failed more than this value will be ignored by the repository
        /// </summary>
        int FailureCountToIgnore { get; set; }
        void EnsureStorage();
    }
}