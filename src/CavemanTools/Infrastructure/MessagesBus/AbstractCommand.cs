using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public abstract class AbstractCommand : ICommand
    {
        public AbstractCommand()
        {
            Id = Guid.NewGuid();
            SourceId = Id;
            TimeStamp = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public DateTime TimeStamp { get;  set; }
        
        public Guid? SourceId { get; set; }
        public void Enrol(IEvent evnt)
        {
            evnt.SourceId = Id;
        }

        public T CreateEvent<T>(Action<T> constructor) where T : IEvent,new()
        {
            var res = new T();
            constructor(res);
            Enrol(res);
            return res;
        }
    }
}