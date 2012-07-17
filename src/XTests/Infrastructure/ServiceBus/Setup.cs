using CavemanTools.Infrastructure;

namespace XTests.Infrastructure.ServiceBus
{
    public class Setup
    {
         
    }

    public class MyCommand:ICommand
    {
        
    }

    public class MyChildCommand:MyCommand
    {
        
    }

    public class MyEvent:IEvent
    {
        
    }

    public class MyChildEvent:MyEvent
    {
        
    }

    public class MyError:AbstractErrorMessage
    {
        
    }
}