using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public abstract class AbstractEvent:IEvent
    {
        public AbstractEvent()
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public DateTime TimeStamp { get;set; }

        public Guid? SourceId
        { get; set; }

        public T CreateCommand<T>(Action<T> constructor) where T : ICommand, new()
        {
            var res = new T();
            constructor(res);
            res.SourceId = Id;
            return res;
        }
    }
}