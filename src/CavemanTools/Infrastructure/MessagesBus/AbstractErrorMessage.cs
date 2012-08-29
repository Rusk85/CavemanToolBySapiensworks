using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public abstract class AbstractErrorMessage : Exception, IMessage
    {
        public Guid Id { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public Guid? SourceId
        {
            get;
            set;
        }
    }
}