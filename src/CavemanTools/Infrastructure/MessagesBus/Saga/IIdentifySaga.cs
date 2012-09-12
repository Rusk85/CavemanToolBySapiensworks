namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface IIdentifySaga
    {
        string SagaCorrelationId { get; }
    }
}