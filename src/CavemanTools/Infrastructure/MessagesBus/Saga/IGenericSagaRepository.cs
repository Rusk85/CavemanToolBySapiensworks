using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public interface IGenericSagaRepository
    {
        void Save(ISagaState state);
        /// <summary>
        /// If saga doesn't exists then return a new instance of saga.
        /// THe new instance must have the saga id and correlation id set
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="sagaType"></param>
        /// <returns></returns>
        ISagaState GetSaga(string correlationId, Type sagaType);

        void EnsureStorage();
    }
}