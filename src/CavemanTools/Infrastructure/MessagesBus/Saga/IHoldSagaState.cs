using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface IHoldSagaState
    {
        Guid Id { get; }
        
        bool IsCompleted { get; set; }
    }
}