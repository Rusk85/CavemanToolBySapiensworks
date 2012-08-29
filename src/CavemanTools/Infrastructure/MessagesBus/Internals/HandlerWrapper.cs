using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class HandlerWrapper:MessageHandlerExecutor
    {
        private dynamic _h;

        public HandlerWrapper(Type messageType) : base(messageType)
        {
        }

        public void Wrap<T>(Action<T> handler) where T:IMessage
        {
            _h = handler;
        }
        

        public override void Handle(IMessage msg)
        {
            _h((dynamic) msg);
        }
    }
}