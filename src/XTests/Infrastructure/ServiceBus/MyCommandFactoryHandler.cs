using System;
using CavemanTools.Infrastructure.MessagesBus;

namespace XTests.Infrastructure.ServiceBus
{
    public class MyCommandFactoryHandler:IExecuteCommand<MyCommand>,ISubscribeToEvent<MyEvent>
    {
        public void Execute(MyCommand cmd)
        {
            Console.WriteLine("command received");
        }

        public int CommandResult { get; set; }
        public int EventResult { get; set; }

        public void Handle(MyEvent evnt)
        {
            Console.WriteLine("Event trigered");
        }
    }
}