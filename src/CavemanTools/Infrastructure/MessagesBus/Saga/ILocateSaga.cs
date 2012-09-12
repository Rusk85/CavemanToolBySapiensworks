namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public class ILocateSagaState<T> where T:IHoldSagaState
    {
        public interface Using<E> where E:IEvent
        {
            T Locate(E message);
        }
    }

    public interface IPersistSagaState
    {
        void Save(IHoldSagaState state);
    }
}