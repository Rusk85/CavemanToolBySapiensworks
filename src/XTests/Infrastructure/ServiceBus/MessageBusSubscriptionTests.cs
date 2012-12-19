﻿using System.Linq;
using CavemanTools.Infrastructure;
using CavemanTools.Infrastructure.MessagesBus;
using CavemanTools.Infrastructure.MessagesBus.Internals;
using CavemanTools.Infrastructure.MessagesBus.Saga;
using CavemanTools.Logging;
using Moq;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.ServiceBus
{
   class MyHandler1:IExecuteCommand<MyCommand>
   {
       public void Execute(MyCommand cmd)
       {
           Console.WriteLine(cmd.Test);
       }
   }

    public class TypedEventHandler:ISubscribeToEvent<MySagaEvent>
    {
        public void Handle(MySagaEvent evnt)
        {
            
        }
    }

    public class MessageBusSubscriptionTests:
        IExecuteCommand<MyCommand>
        ,ISubscribeToEvent<MyEvent>
    {
        private Stopwatch _t = new Stopwatch();
        private LocalMessageBus _bus;
        

        public MessageBusSubscriptionTests()
        {
            LogSetup.ToConsole();
            var store = new Mock<IStoreMessageBusState>();
            var resolver = new Mock<IResolveSagaRepositories>();
            var container = ActivatorContainer.Instance;
            _bus = new LocalMessageBus(store.Object, container, resolver.Object, LogHelper.DefaultLogger);
        }

        [Fact]
        public void internal_messageHandlerDiscoverer_doesnt_consider_both_saga_and_event()
        {
            var all = MessageHandlerDiscoverer.GetImplementedInterfaces(typeof (SagaTests));
            Assert.Equal(4,all.Count());
        }

        [Fact]
        public void registering_handler_by_type_needs_public_class()
        {
            Assert.Throws<ArgumentException>(() => _bus.RegisterHandlerType(typeof(MyCommand), typeof(MyHandler1)));                        
        }

        [Fact]
        public void any_new_command_has_defaults()
        {
            var m = new MyCommand();
            Assert.NotEqual(Guid.Empty,m.Id);
            Assert.NotNull(m.SourceId);
            Assert.NotEqual(default(DateTime),m.TimeStamp);
        }


        [Fact]
        public void any_new_event_has_defaults()
        {
            var m = new MyEvent();
            Assert.NotEqual(Guid.Empty, m.Id);
            Assert.Null(m.SourceId);
            Assert.NotEqual(default(DateTime), m.TimeStamp);
        }

        [Fact]
        public void create_event_from_command()
        {
            var cmd = new MyCommand();
            var ev=cmd.CreateEvent<MyEvent>(e => { });
            Assert.Equal(cmd.Id,ev.SourceId.Value);

            var ev2 = new MyEvent();
            cmd.Enrol(ev2);
            Assert.Equal(cmd.Id,ev2.SourceId.Value);
        }

        [Fact]
        public void create_command_from_event()
        {
            var ev = new MyEvent() {SourceId = Guid.Empty};
            var cmd = ev.CreateCommand<MyCommand>(c => { });
            Assert.Equal(ev.Id,cmd.SourceId);
        }

        [Fact]
        public void command_lambda_handler_is_registered()
        {
            var d = _bus.SetupCommandHandler((MyCommand cmd) => { });
            Assert.NotNull(_bus.Subscriptions.Get(typeof(MyCommand)));
            d.Dispose();
            Assert.Null(_bus.Subscriptions.Get(typeof(MyCommand)));
        }

        [Fact]
        public void event_lambda_handler_is_registered()
        {
            var d = _bus.SetupEventHandler((MyEvent cmd) => { });
            var h = _bus.Subscriptions.Get(typeof (MyEvent)) as EventExecutor;
            Assert.NotNull(h);
            Assert.Equal(1,h.Subs.Count);
            d.Dispose();
            Assert.Equal(0, h.Subs.Count);
        }

        [Fact]
        public void register_object()
        {
            var d=_bus.RegisterHandler(typeof (MyCommand), this);
            var h = _bus.Subscriptions.Get(typeof (MyCommand));
            Assert.NotNull(h);
            d.Dispose();
            Assert.Null(_bus.Subscriptions.Get(typeof(MyCommand)));

            d = _bus.RegisterHandler(typeof (MyEvent), this);
            Assert.Equal(1,_bus.Subscriptions.Get(typeof(MyEvent)).Cast<EventExecutor>().Subs.Count);
        }

        [Fact]
        public void only_one_command_handler_is_permitted()
        {
            _bus.SetupCommandHandler<MyCommand>(c => { });
            Assert.Throws<DuplicateCommandHandlerException>(() =>
                                                                {
                                                                    _bus.SetupCommandHandler<MyCommand>(c => { });
                                                                });
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }

        public void Execute(MyCommand cmd)
        {
            throw new NotImplementedException();
        }

        public void Handle(MyEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}