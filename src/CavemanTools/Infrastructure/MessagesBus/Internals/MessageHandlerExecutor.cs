using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal abstract class MessageHandlerExecutor:IExecuteMessageHandler,IDisposable        
    {
        public Type MessageType { get; private set; }
        //protected readonly IStoreServiceBusState Storage;
        
        
        public MessageHandlerExecutor(Type messageType)
        {
            MessageType = messageType;                      
        }

        public bool CanHandle(Type evnt)
        {
            return MessageType.IsAssignableFrom(evnt);
        }

        public bool IsExactlyFor(Type evnt)
        {
            return MessageType.Equals(evnt);
        }

        public abstract void Handle(IMessage msg);

        public virtual dynamic ExecuteRequest(IMessage msg)
        {
            throw new NotSupportedException();
        }

        public virtual void Dispose()
        {
            
        }
    }
}