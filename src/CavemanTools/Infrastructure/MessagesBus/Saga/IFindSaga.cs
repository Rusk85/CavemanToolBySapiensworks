namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public abstract class IFindSaga<T> where T:ISagaState
    {
        public interface Using<V> where V:IEvent
        {
            /// <summary>
            /// If saga is not found, a new instance must be returned
            /// </summary>
            /// <param name="evnt"></param>
            /// <returns></returns>
            T FindSaga(V evnt);
        }

        
    }
}