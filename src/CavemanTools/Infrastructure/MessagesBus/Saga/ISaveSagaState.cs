namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface ISaveSagaState<T> where T:IHoldSagaState
    {
        void Save(T saga);        
    }
}