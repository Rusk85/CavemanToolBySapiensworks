namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal interface IExecuteMessageHandler
    {
        void Handle(IMessage msg);
    }
}