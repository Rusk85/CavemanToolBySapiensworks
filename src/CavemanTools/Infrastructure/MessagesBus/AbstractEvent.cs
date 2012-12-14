using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public abstract class AbstractEvent:IEvent
    {
        private Guid? _sourceId;

        public AbstractEvent()
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public DateTime TimeStamp { get;set; }

        public Guid? SourceId
        {
            get { return _sourceId; }
            set
            {
                if (value == null) throw new ArgumentNullException("value", "SourceId must always have a value");
                _sourceId = value;
            }
        }

        public T CreateCommand<T>(Action<T> constructor) where T : ICommand, new()
        {
            var res = new T();
            constructor(res);
            res.SourceId = Id;
            return res;
        }
    }
}