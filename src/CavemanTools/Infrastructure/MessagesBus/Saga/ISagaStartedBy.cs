namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface ISagaStartedBy<T>:IHandleEvent<T> where T:IEvent
    {
        
    }

    
}