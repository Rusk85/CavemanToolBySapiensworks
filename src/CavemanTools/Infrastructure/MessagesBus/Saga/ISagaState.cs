using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface ISagaState:IIdentifySaga
    {
        Guid Id { get; set; }
        
        bool IsCompleted { get; set; }
    }
}