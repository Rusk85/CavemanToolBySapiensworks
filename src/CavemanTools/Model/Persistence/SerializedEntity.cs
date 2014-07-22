using System;

namespace CavemanTools.Model.Persistence
{
    public class SerializedEntity
    {
        public Guid EntityId { get; set; }
        public string Data { get; set; }
        public int Version { get; set; }
    }
}