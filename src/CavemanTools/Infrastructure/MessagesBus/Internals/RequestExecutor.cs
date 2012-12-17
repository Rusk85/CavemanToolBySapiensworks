using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class RequestExecutor:CommandExecutor
    {
        public RequestExecutor(Type messageType, Func<IInvokeMessageHandler> handler) : base(messageType, handler)
        {
        }

        public override dynamic ExecuteRequest(IMessage msg)
        {
            return _handlerFactory().ExecuteRequest(msg as ICommand);
        }
    }
}