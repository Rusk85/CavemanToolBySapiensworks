using CavemanTools.Infrastructure;
using CavemanTools.Infrastructure.MessagesBus;

namespace XTests.Infrastructure.ServiceBus
{
    public class Setup
    {
         
    }

    public class MyCommand:AbstractCommand
    {
        public int Test { get; set; }
        public override string ToString()
        {
            return base.ToString()+" Test: "+Test;
        }
    }

    public class MyChildCommand:MyCommand
    {
        
    }

    public class MyEvent:AbstractEvent
    {
        public int Test { get; set; }
    }

    public class MyChildEvent:MyEvent
    {
        
    }

    public class MyError:AbstractErrorMessage
    {
        
    }
}