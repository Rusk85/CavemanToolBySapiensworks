using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface IGenericSagaRepository
    {
        void Save(IIdentifySaga state);

        IHoldSagaState GetSaga(string correlationId, Type sagaType);    
    }
}