using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class RequestExecutor:CommandExecutor
    {
        public RequestExecutor(Type messageType, object handler) : base(messageType, handler)
        {
        }

        public override void Handle(IMessage msg)
        {
            ExecuteRequest(msg);
        }

        public override dynamic ExecuteRequest(IMessage msg)
        {
            return _handler.Execute((dynamic) msg);
        }
    }
}