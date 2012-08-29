using System;
using System.Collections.Generic;
using System.Linq;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public abstract class AggregateRoot<T> where T:IEvent
    {
        List<T> _newEvents = new List<T>();
        private Guid? _operationId;
        public Guid RootId { get; protected set; }

        private IEnumerable<T> _history=new T[0];

        public AggregateRoot()
        {
            IgnoreOperationId = false;
        }

        public AggregateRoot(Guid id, IEnumerable<T> events)
        {
            RootId = id;
            foreach (var ev in events)
            {                
                ApplyChange(ev, false);
            }

            _history = events;
        }


        protected bool IgnoreOperationId { get; set; }

        public void SetOperationId(Guid id)
        {
            if (_history.Any(e=>e.SourceId==id))
            {
                throw new DuplicateOperationException();
            }
            _operationId = id;
        }

        protected AggregateRoot(Guid id)
        {
            RootId = id;
        }
        protected abstract void PlayEvent(T ev);

        public void MarkChangesAsCommited()
        {
            _newEvents.Clear();
            _operationId = null;
        }

        public IEnumerable<T> GetChanges()
        {
            return _newEvents;
        }

        public IEnumerable<T> GetEventsForOperation(Guid id)
        {
            return _history.Where(e => e.SourceId == id);
        }

        protected void ApplyChange(T ev, bool isNew = true)
        {
            PlayEvent(ev);
            if (isNew)
            {
                if (!IgnoreOperationId && _operationId == null) throw new InvalidOperationException("You have to set the operation id first");
                ev.SourceId = _operationId.Value;
                _newEvents.Add(ev);                
            }
            
        }



    }
}