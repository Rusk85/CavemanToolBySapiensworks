using CavemanTools.Infrastructure;
using CavemanTools.Infrastructure.MessagesBus;
using CavemanTools.Infrastructure.MessagesBus.Saga;
using CavemanTools.Logging;
using Moq;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.ServiceBus
{
    public class MessageBusTests:IExecuteCommand<MyCommand>,ISubscribeToEvent<MyEvent>
        ,IExecuteRequest<MyChildCommand,int>        
    {
        private Stopwatch _t = new Stopwatch();
        protected LocalMessageBus _bus;
        protected Mock<IStoreMessageBusState> _storage;

        public MessageBusTests()
        {
            LogSetup.ToConsole();
            _storage = new Mock<IStoreMessageBusState>();
            var resolver = new Mock<IResolveSagaRepositories>();
            var container = new Mock<IContainerScope>();
            _bus = new LocalMessageBus(_storage.Object, container.Object,resolver.Object, LogHelper.DefaultLogger);
            SetupBus();
           
           
        }

        protected virtual void SetupBus()
        {
            _bus.RegisterHandler<MyCommand>(this);
            _bus.RegisterHandler<MyEvent>(this);
            _bus.RegisterHandler<MyChildCommand, int>(this); 
        }

        private bool _flag;
        
        #region Commands
        [Fact]
        public void command_handler_is_called()
        {
            _flag = false;
            _bus.Send(new MyCommand() { Test = 75 });
            Assert.True(_flag);
        }

        [Fact]
        public void any_new_command_is_persisted()
        {
            _flag = false;
            var cmd = new MyCommand() { Test = 75 };
            _bus.Send(cmd);
            _storage.Verify(d => d.StoreMessageInProgress(cmd), Times.Once());
        }

        [Fact]
        public void any_completed_command_is_persisted()
        {
            _flag = false;
            var cmd = new MyCommand() { Test = 75 };
            _bus.Send(cmd);
            _storage.Verify(d => d.MarkMessageCompleted(cmd.Id), Times.Once());
        }

        [Fact]
        public void missing_command_handler_throws_by_default()
        {
            _bus.Subscriptions.Clear();
            Assert.Throws<MissingCommandHandlerException>(() => _bus.Send(new MyCommand()));
        }

        [Fact]
        public void missing_command_handler_is_ignored()
        {
            _bus.Subscriptions.Clear();
            _bus.IgnoreMissingCommandHandler = true;
            Assert.DoesNotThrow(() => _bus.Send(new MyCommand()));
        }

        [Fact]
        public void duplicate_command_is_not_executed()
        {
            _flag = false;
            _storage.Setup(d => d.StoreMessageInProgress(It.IsAny<MyCommand>())).Throws<DuplicateMessageException>();
            _bus.Send<MyCommand>(c => c.Test = 75);
            Assert.False(_flag);
        }

        [Fact]
        public void send_async_command_is_executed()
        {
            _flag = false;
            var t = _bus.SendAsync(new MyCommand() { Test = 75 });
            //Assert.False(_flag);
            Write("Waiting...");
            t.Wait();
            Write("Finish waiting");
            Assert.True(_flag);
        } 
        #endregion

        #region Requests

        [Fact]
        public void request_is_executed_and_returns_result()
        {
            var i = 0;
            i+=_bus.Request<int>(new MyChildCommand() {Test = 21});
            Assert.Equal(23,i);
        }

        [Fact]
        public void request_as_command()
        {
            Assert.DoesNotThrow(()=>
                                    {
                                        _bus.Send(new MyChildCommand(){Test = 21});
                                    });
        }

        #endregion

        #region Events

        [Fact]
        public void an_event_without_a_sourceid_throws()
        {
            Assert.Throws<ArgumentException>(() => _bus.PublishAsync(new MyEvent() {Test = 56}));
        }

        [Fact]
        public void event_is_sent_async_to_all_handlers()
        {
            _flag = false;
            int f = 0;
            _bus.SetupEventHandler<MyEvent>(ev =>
                                                {
                                                    f = ev.Test;
                                                    Write("lambda received event {0}",ev);
                                                });

            var t = _bus.PublishAsync(new MyEvent() {Test = 56,SourceId = Guid.NewGuid()});
            t.Wait();
            Assert.Equal(56,f);
            Assert.True(_flag);
        }

        [Fact]
        public void event_is_sent_to_all_handlers()
        {
            _flag = false;
            int f = 0;
            _bus.SetupEventHandler<MyEvent>(ev =>
            {
                f = ev.Test;
                Write("lambda received event {0}", ev);
            });

            _bus.Publish(new MyEvent() { Test = 56, SourceId = Guid.NewGuid() });
            Assert.Equal(56, f);
            Assert.True(_flag);
        }

        [Fact]
        public void multiple_events_are_published_at_once()
        {
            _flag = false;
            int f = 0;
            _bus.SetupEventHandler<MyEvent>(ev =>
            {
                f+= ev.Test;
                Write("lambda received event {0}", ev);
            });

            _bus.SetupEventHandler<MyChildEvent>(ev =>
                                                     {
                                                         f += ev.Test;
                                                     });

            var t = _bus.PublishAsync(new MyEvent()
                                     {
                                         Test = 56, SourceId = Guid.NewGuid()
                                     },
                                     new MyChildEvent(){Test = 10,SourceId = Guid.Empty});
            t.Wait();
            Assert.Equal(66, f);
            Assert.True(_flag);
        }

        [Fact]
        public void any_new_event_is_persisted()
        {
            _flag = false;
            var ev = new MyEvent() { Test = 75,SourceId = Guid.Empty};
            var t=_bus.PublishAsync(ev);
            t.Wait();
            _storage.Verify(d => d.StoreMessageInProgress(ev), Times.Once());
        }

        [Fact]
        public void any_completed_event_is_persisted()
        {
            _flag = false;
            var ev = new MyEvent() { Test = 75 ,SourceId = Guid.Empty};
            var t=_bus.PublishAsync(ev);
            t.Wait();
            _storage.Verify(d => d.MarkMessageCompleted(ev.Id), Times.Once());
        }

        [Fact]
        public void duplicate_event_is_not_executed()
        {
            _flag = false;
            _storage.Setup(d => d.StoreMessageInProgress(It.IsAny<MyEvent>())).Throws<DuplicateMessageException>();
            var t=_bus.PublishAsync(new MyEvent() {Test=56,SourceId = Guid.Empty});
            t.Wait();
            Assert.False(_flag);
        }

        [Fact]
        public void throw_exception_in_event_handler()
        {
            _bus.OnUnhandledException = x => { throw x; };
            var t =_bus.PublishAsync(new MyEvent() {SourceId = Guid.Empty, Test = 34});
            Assert.Throws<AggregateException>(() => t.Wait());                        
        }

        #endregion

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }

        public void Execute(MyCommand cmd)
        {
            _flag = cmd.Test == 75;
        }

        public void Handle(MyEvent evnt)
        {
            _flag = evnt.Test == 56;
            Write("handler received {0}",evnt);
            if (evnt.Test==34)
            {
                throw new NotImplementedException();
            }
        }

        public int Execute(MyChildCommand cmd)
        {
            return cmd.Test + 2;
        }
    }
}