using System;
using System.Collections.Generic;
using System.Linq;

namespace CavemanTools.Model.Persistence
{
    public class MemoryCollection : List<object>
    {
        public class VersionedItem<T> where T:IEntity
        {
            public Guid Id { get; set; }
            public T Value { get; set; }
            public int Version { get; set; }

            public VersionedItem(T value)
            {
                Id = value.Id;
                Value = value;
                Version = 1;
            }
        }

        public IEnumerable<T> GetItems<T>() where T : IEntity
        {
            return this.OfType<VersionedItem<T>>().Select(d => d.Value).Union(this.OfType<T>());
        }
    }
}