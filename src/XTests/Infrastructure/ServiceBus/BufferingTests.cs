using CavemanTools.Infrastructure;
using Xunit;
namespace XTests.Infrastructure.ServiceBus
{
    public class BufferingTests
    {
          private IBus _bus;

        public BufferingTests()
        {
            _bus=new CavemanTools.Infrastructure.ServiceBus();
        }

        [Fact]
        public void buffered_messages_are_not_sent()
        {
            var rez = false;
            _bus.BeginBuffering();
            _bus.RegisterCommandHandlerFor<MyCommand>(c => rez = true);
            _bus.Send(new MyCommand());
            Assert.False(rez);
        }

        [Fact]
        public void buffered_messages_are_sent_when_flushed()
        {
            var rez = false;
            _bus.BeginBuffering();
            _bus.RegisterCommandHandlerFor<MyCommand>(c => rez = true);
            _bus.Send(new MyCommand());
            Assert.False(rez);
            _bus.FlushBuffer();
            Assert.True(rez);
        }
        
        [Fact]
        public void buffered_messages_are_sent_when_flushed_the_using_version()
        {
            var rez = false;
            _bus.RegisterCommandHandlerFor<MyCommand>(c => rez = true);
            using(var b=_bus.BeginBuffering())
            {
                _bus.Send(new MyCommand());
                Assert.False(rez);
                b.Flush();
            }
            
            Assert.True(rez);
        }

        [Fact]
        public void clearing_buffer_doesn_not_send_messages()
        {
            var rez = false;
            _bus.RegisterCommandHandlerFor<MyCommand>(c => rez = true);
            using(_bus.BeginBuffering())
            {
                _bus.Send(new MyCommand());
            }
            Assert.False(_bus.IsBuffering);
            Assert.False(rez);
        }

    }
}