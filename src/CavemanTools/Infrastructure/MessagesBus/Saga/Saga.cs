namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public abstract class Saga<T>:ISaga<T> where T : IHoldSagaState
    {
        public T SagaState
        { get; set; }

       protected void MarkAsComplete()
        {
            SagaState.IsCompleted = true;
        }
    }
}