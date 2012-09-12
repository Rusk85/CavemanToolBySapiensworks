namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    //public interface ISaga
    //{
    //    IHoldSagaState SagaData { get; set; }
    //}
    public interface ISaga<T> where T:IHoldSagaState
    {
        T SagaState { get; set; }
    }
}