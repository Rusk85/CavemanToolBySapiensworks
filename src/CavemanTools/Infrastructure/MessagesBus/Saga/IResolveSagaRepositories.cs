using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface IResolveSagaRepositories
    {
        IGenericSagaRepository GetGenericRepository();
        dynamic GetSagaLocator(Type sagaType, Type eventType);
        dynamic GetSagaPersister(Type sagaType);
    }
}