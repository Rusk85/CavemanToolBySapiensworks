using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class CommandExecutor:MessageHandlerExecutor
    {
        protected readonly Func<IInvokeMessageHandler> _handlerFactory;
     
        public CommandExecutor(Type messageType,Func<IInvokeMessageHandler> handlerFactory) : base(messageType)
        {
            _handlerFactory = handlerFactory;
        }

        public override void Handle(IMessage msg)
        {
           _handlerFactory().Execute(msg);
        }
     
    }
}