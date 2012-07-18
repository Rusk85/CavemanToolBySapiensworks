using System.Reflection;
using Xunit;
using System;
using System.Diagnostics;
using CavemanTools.Infrastructure;

namespace XTests.Infrastructure.ServiceBus
{
   public class CommandHandlers:IHandleCommand<MyCommand>,IHandleCommand<MyChildCommand>
       ,IHandleEvent<MyEvent>
       ,IHandleEvent<MyChildEvent>
   {
       public bool Mc = false;
       public bool McC = false;
       public bool Me = false;
       public bool MeC = false;
       public void Handle(MyCommand cmd)
       {
           Mc = true;
       }

       public void Handle(MyChildCommand cmd)
       {
           McC = true;
       }

       public void Handle(MyEvent evnt)
       {
           Me = true;
       }

       public void Handle(MyChildEvent evnt)
       {
           MeC = true;
       }
   }
    public class ServiceBusExtensionTests
    {
        private Stopwatch _t = new Stopwatch();
        private CavemanTools.Infrastructure.ServiceBus _bus;
        private CommandHandlers _myhandlers;

        public ServiceBusExtensionTests()
        {
            _bus = new CavemanTools.Infrastructure.ServiceBus();
            _myhandlers = new CommandHandlers();
        }

        [Fact]
        public void register_commands()
        {
            _bus.RegisterHandlersFromAssemblyOf<ServiceBusExtensionTests>(Create);
            Assert.False(_myhandlers.Mc);
            Assert.False(_myhandlers.McC);
            _bus.Send(new MyCommand());
            Assert.True(_myhandlers.Mc);
            _bus.Send(new MyChildCommand());
            Assert.True(_myhandlers.McC);
        }
        
        [Fact]
        public void register_events()
        {
            _bus.RegisterSubscribersFromAssemblyOf<ServiceBusExtensionTests>(Create);
            Assert.False(_myhandlers.Me);
            Assert.False(_myhandlers.MeC);
            _bus.Publish(new MyEvent());
            Assert.True(_myhandlers.Me);
            _bus.Publish(new MyChildEvent());
            Assert.True(_myhandlers.MeC);
        }

        object Create(Type t)
        {
            return _myhandlers;
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}