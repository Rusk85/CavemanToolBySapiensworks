using System.Collections.Generic;

namespace CavemanTools.Infrastructure.Internals
{
    internal class Buffer:IBufferMessages
    {
     internal struct BufferedMessage
     {
         public IMessage Message;
         public HandlingType Handling;

         
     }
        private ServiceBus _parent;

        public Buffer(ServiceBus parent)
        {
            _parent = parent;
        }

        private List<BufferedMessage> _bufferedEvents= new List<BufferedMessage>();

        public void Add(IMessage evnt,HandlingType handling)
        {
            var bm = new BufferedMessage();
            bm.Message = evnt;
            bm.Handling = handling;
            _bufferedEvents.Add(bm);
        }

        public void Publish()
        {
            foreach (var evnt in _bufferedEvents)
            {
                _parent.SendMessage(evnt.Message,evnt.Handling,true);
            }
        }

        public void Dispose()
        {
            _parent.ClearBuffer();
            _bufferedEvents.Clear();
        }

        public void Flush()
        {
            _parent.FlushBuffer();
        }
    }
}