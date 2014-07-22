using System;

namespace CavemanTools.Model.Persistence
{
    public class IdempotencyHistoryItem
    {
        public Guid EntityId { get; set; }
        public Guid OperationId { get; set; }
        public string GeneratedEvents { get; set; }
    }
}