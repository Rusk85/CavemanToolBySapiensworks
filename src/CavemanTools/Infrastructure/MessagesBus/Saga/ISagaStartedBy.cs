namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface ISagaStartedBy<T>:ISubscribeToEvent<T> where T:IEvent
    {
        
    }

    
}