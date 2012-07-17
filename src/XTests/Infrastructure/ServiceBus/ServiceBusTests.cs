using CavemanTools.Infrastructure;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.ServiceBus
{
    public class ServiceBusTests
    {
        private Stopwatch _t = new Stopwatch();
        private IBus _bus;

        public ServiceBusTests()
        {
            _bus=new CavemanTools.Infrastructure.ServiceBus();
        }

        [Fact]
        public void only_one_command_handler_is_permitted()
        {
            _bus.RegisterCommandHandlerFor<MyCommand>(c => { });
            Assert.Throws<DuplicateCommandHandlerException>(() =>
                                                                {
                                                                    _bus.RegisterCommandHandlerFor<MyCommand>(c => { });
                                                                });
        }

        [Fact]
        public void command_is_sent()
        {
            bool rez = false;
            _bus.RegisterCommandHandlerFor((MyCommand c) => { rez = true; });
            _bus.Send(new MyCommand());
            Assert.True(rez);
        }

        [Fact]
        public void only_specific_command_is_handled()
        {
            bool rez = false;
            bool crez = false;
            _bus.RegisterCommandHandlerFor((MyCommand c) => { rez = true; });
            _bus.RegisterCommandHandlerFor((MyChildCommand c) => { crez = true; });
            _bus.Send(new MyChildCommand());
            Assert.True(crez);
            Assert.False(rez);
        }

        [Fact]
        public void event_publishing_to_all_subscribers()
        {
            var rez1 = false;
            var rez2 = false;
            _bus.SubscribeToEvent((MyEvent ev) => { rez1 = true; });
            _bus.SubscribeToEvent((MyEvent ev) => { rez2 = true; });
            _bus.Publish(new MyEvent());
            Assert.True(rez1);
            Assert.True(rez2);
        }

        [Fact]
        public void event_handled_by_specific_and_ancestor_handlers()
        {
            var rez1 = false;
            var rez2 = false;
            _bus.SubscribeToEvent((MyEvent ev) => { rez1 = true; });
            _bus.SubscribeToEvent((MyChildEvent ev) => { rez2 = true; });
            _bus.Publish(new MyChildEvent(),HandlingType.AllDescendants);
            Assert.True(rez1);
            Assert.True(rez2); 
        }

        [Fact]
        public void only_specific_event_is_handled()
        {
            var rez1 = false;
            var rez2 = false;
            _bus.SubscribeToEvent((MyEvent ev) => { rez1 = true; });
            _bus.SubscribeToEvent((MyChildEvent ev) => { rez2 = true; });
            _bus.Publish(new MyChildEvent());
            Assert.False(rez1);
            Assert.True(rez2);
        }

        [Fact]
        public void notify_error()
        {
            var rez = false;
            _bus.SubscribeToError<MyError>(e => { rez = true; });
            _bus.Notify(new MyError());
            Assert.True(rez);
        }

        [Fact]
        public void no_error_subscribers_throws()
        {
            Assert.Throws<NoSubscribersException>(() =>
                                                      {
                                                          _bus.Notify(new MyError());
                                                      });
        }

        [Fact]
        public void ignore_no_error_subscribers()
        {
            _bus.IgnoreLackOfSubscribers = true;
            Assert.DoesNotThrow(() =>
            {
                _bus.Notify(new MyError());
            });
        }

        [Fact]
        public void exception_unhandled_by_handler_is_thrown()
        {
            _bus.RegisterCommandHandlerFor((MyCommand c) => { throw new Exception();});
            Assert.Throws<Exception>(() =>
                                         {
                                             _bus.Send(new MyCommand());
                                         });
        }

        [Fact]
        public void when_set_exception_unhandled_by_handler_is_converted_to_notification()
        {
            var rez = false;
            _bus.ThrowOnUnhandledExceptions = false;
            _bus.RegisterCommandHandlerFor((MyCommand c) => { throw new Exception(); });
            _bus.SubscribeToError<UnhandledMessageException>(e => { rez = true; });
            
            Assert.DoesNotThrow(()=>
                                    {
                                        _bus.Send(new MyCommand());
                                    });
            Assert.True(rez);
        }

        [Fact]
        public void remove_handler()
        {
            var rez = false;
            var d = _bus.RegisterCommandHandlerFor((MyCommand c) => rez = true);
            d.Dispose();
            _bus.Send(new MyCommand());
            Assert.False(rez);
        }

        [Fact]
        public void spawn_local_contains_handlers_of_parent_bus()
        {
            var rez = false;
            var d=_bus.RegisterCommandHandlerFor((MyCommand c) => rez = true);
            var b = _bus.SpawnLocal();
            d.Dispose();
            b.Send(new MyCommand());
            Assert.True(rez);
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}