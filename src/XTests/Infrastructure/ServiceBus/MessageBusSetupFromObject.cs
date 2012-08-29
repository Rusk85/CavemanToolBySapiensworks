using CavemanTools.Infrastructure.MessagesBus.Internals;

namespace XTests.Infrastructure.ServiceBus
{
    public class MessageBusSetupFromObject:MessageBusTests
    {
        protected override void  SetupBus()
        {
            _bus.RegisterHandler(typeof(MyCommand), this);
            _bus.RegisterHandler(typeof(MyEvent), this);
            _bus.RegisterHandler(typeof(MyChildCommand), this);
        }
    }
}