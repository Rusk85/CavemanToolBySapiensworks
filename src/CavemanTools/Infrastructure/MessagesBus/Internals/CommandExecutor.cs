using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class CommandExecutor:MessageHandlerExecutor
    {
        protected readonly dynamic _handler;

        public CommandExecutor(Type messageType,object handler) : base(messageType)
        {
            _handler = handler;
        }

        public override void Handle(IMessage msg)
        {
            _handler.Execute((dynamic) msg);
        }
     
    }
}