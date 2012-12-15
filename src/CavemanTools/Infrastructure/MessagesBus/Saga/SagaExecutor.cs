using System;
using System.Linq;
using CavemanTools.Infrastructure.MessagesBus.Internals;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    internal class SagaExecutor:IExecuteMessageHandler
    {
        //private readonly Type _message;
        private dynamic _handler;
        private readonly IResolveSagaRepositories _resolver;
        private readonly IDispatchCommands _bus;
        private Type _sagaType;
        public SagaExecutor(object handler,IResolveSagaRepositories resolver,IDispatchCommands bus)
        {
           // _message = message;
            _handler = handler;
            _resolver = resolver;
            _bus = bus;
            ExtractSagaType();
        }

        void ExtractSagaType()
        {
            var o = (_handler as Object).GetType();
            var sagaInterface =
                o.GetInterfaces().Where(i => i.IsGenericType && i.Name.StartsWith("ISaga")).FirstOrDefault();
            if (sagaInterface!=null)
            {
                _sagaType = sagaInterface.GetGenericArguments()[0];
                return;
            }
            throw new InvalidOperationException("Object is not a valid saga");
        }

        dynamic GetSaga(IEvent evnt)
        {
            dynamic repo = null;

            repo = _resolver.GetSagaLocator(_sagaType, evnt.GetType());
            if (repo != null)
            {
                return repo.FindSaga((dynamic)evnt);
            }

            repo = _resolver.GetGenericRepository();
            if (!(evnt is IIdentifySaga))
            {
                throw new InvalidOperationException("When using the default saga finder, the event must implement IIdentifySaga");
            }
            var sagaIdentifier = evnt as IIdentifySaga;
            return repo.GetSaga(sagaIdentifier.SagaCorrelationId, _sagaType);
        }

        void SaveSaga(dynamic saga)
        {
            dynamic repo = null;
            repo = _resolver.GetSagaPersister(_sagaType);
            
            if (repo==null)
            {
                repo = _resolver.GetGenericRepository();
            }
            repo.Save(saga);
        }

        public void Handle(IMessage msg)
        {
            var data = GetSaga(msg as IEvent);
            if (data.IsCompleted) return;
            _handler.Data = data;
            _handler.SetBus(_bus);
            _handler.Handle((dynamic) msg);
            SaveSaga(data);
        }
    }
}