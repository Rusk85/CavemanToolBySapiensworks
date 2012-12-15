namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface IIdentifySaga
    {
        /// <summary>
        /// When using the generic saga repository, this must have a value which identifies the correct saga
        /// </summary>
        string SagaCorrelationId { get; set; }
    }
}