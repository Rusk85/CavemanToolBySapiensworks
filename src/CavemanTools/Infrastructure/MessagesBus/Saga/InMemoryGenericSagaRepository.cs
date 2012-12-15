using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public class InMemoryGenericSagaRepository:IGenericSagaRepository
    {
        private Dictionary<Tuple<string, Type>, ISagaState> _list;

        public InMemoryGenericSagaRepository()
        {
            _list = new Dictionary<Tuple<string, Type>, ISagaState>();
        }
        public void Save(ISagaState state)
        {
            state.MustNotBeNull();
            var key = new Tuple<string, Type>(state.SagaCorrelationId, state.GetType());
            _list[key] = state;
        }

        public ISagaState GetSaga(string correlationId, Type sagaType)
        {
            var key=new Tuple<string, Type>(correlationId,sagaType);
            if (_list.ContainsKey(key))
            {
                return _list[key];
            }
            return (ISagaState) Activator.CreateInstance(sagaType);
        }
    }
}