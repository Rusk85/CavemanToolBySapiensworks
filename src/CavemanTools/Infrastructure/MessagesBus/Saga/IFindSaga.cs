namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public abstract class IFindSaga<T> where T:IHoldSagaState
    {
        public interface Using<V> where V:IEvent
        {
            T FindSaga(V evnt);
        }

        
    }
}