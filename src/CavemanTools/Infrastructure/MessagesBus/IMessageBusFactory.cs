namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IMessageBusFactory
    {
        void PerformRecovery();
        IMessageBus CreateBus();
    }
}