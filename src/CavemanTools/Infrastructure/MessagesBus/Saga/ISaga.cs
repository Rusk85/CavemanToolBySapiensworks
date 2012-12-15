namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    //public interface ISaga
    //{
    //    IHoldSagaState SagaData { get; set; }
    //}
    public interface ISaga<T> where T:ISagaState
    {
        T Data { get; set; }
    }
}