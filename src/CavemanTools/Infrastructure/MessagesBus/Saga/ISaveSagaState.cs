namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface ISaveSagaState<T> where T:ISagaState
    {
        void Save(T saga);        
    }
}