using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public class InMemoryGenericSagaRepository:IGenericSagaRepository
    {
        private Dictionary<Tuple<string, Type>, IHoldSagaState> _list;

        public InMemoryGenericSagaRepository()
        {
            _list = new Dictionary<Tuple<string, Type>, IHoldSagaState>();
        }
        public void Save(IIdentifySaga state)
        {
            var key = new Tuple<string, Type>(state.SagaCorrelationId, state.GetType());
            if (!(state is IHoldSagaState)) throw new ArgumentException("saga state must implement IHoldSageState");
            _list[key] = state as IHoldSagaState;
        }

        public IHoldSagaState GetSaga(string correlationId, Type sagaType)
        {
            var key=new Tuple<string, Type>(correlationId,sagaType);
            if (_list.ContainsKey(key))
            {
                return _list[key];
            }
            return (IHoldSagaState) Activator.CreateInstance(sagaType);
        }
    }
}