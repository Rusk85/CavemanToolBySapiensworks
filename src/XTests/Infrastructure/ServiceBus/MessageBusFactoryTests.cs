using System;
using CavemanTools.Infrastructure.MessagesBus;
using CavemanTools.Logging;
using Moq;
using Xunit;

namespace XTests.Infrastructure.ServiceBus
{
    public class MessageBusFactoryTests
    {
        private Mock<IStoreMessageBusState> _storage;
        private IMessageBusFactory _busFactory;

        public MessageBusFactoryTests()
        {
            LogSetup.ToConsole();
            _storage = new Mock<IStoreMessageBusState>();
            _busFactory = MessageBusFactory.Configure()
                .WithLogger(LogHelper.DefaultLogger)
                .WithStorageFactory(()=>_storage.Object)
                .UseDefaultTypeActivator()
                .UseHandlersFrom<MyCommandFactoryHandler>()
                .Build();
        }

        [Fact]
        public void create_bus()
        {
            var bus = _busFactory.CreateBus() as LocalMessageBus;
            Assert.Equal(2,bus.Subscriptions.Count);
            Assert.DoesNotThrow(()=>
                                    {
                                        bus.Send<MyCommand>(c => { c.Test = 3; });
                                        bus.Publish(new MyEvent() { SourceId = Guid.Empty });
                                    });
            
        }

        [Fact]
        public void do_recovery()
        {
            var cmd = new MyCommand();
            var cmdId = cmd.Id;
            var ev = cmd.CreateEvent<MyEvent>(c => { });
            var evId = ev.Id;
            _storage.Setup(d => d.GetUncompletedMessages(It.IsAny<int>())).Returns(new IMessage[] {cmd, ev});
            _busFactory.PerformRecovery();
            _storage.Verify(d=>d.MarkMessageCompleted(evId),Times.Once());
            _storage.Verify(d=>d.MarkMessageCompleted(cmdId),Times.Once());
            _storage.Verify(d=>d.StoreMessageInProgress(It.IsAny<IMessage>()),Times.Never());

        }
     
    }
}