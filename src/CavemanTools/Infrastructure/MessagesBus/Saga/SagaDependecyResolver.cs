using System;

namespace CavemanTools.Infrastructure.MessagesBus.Saga
{
    public class SagaDependecyResolver : IResolveSagaRepositories
    {
        private readonly IResolveDependencies _resolver;

        public SagaDependecyResolver(IResolveDependencies resolver)
        {
            _resolver = resolver;
        }

        public IGenericSagaRepository GetGenericRepository()
        {
            return _resolver.Resolve<IGenericSagaRepository>();
        }

        public dynamic GetSagaLocator(Type sagaType, Type eventType)
        {
            return _resolver.Resolve(typeof (IFindSaga<>.Using<>).MakeGenericType(sagaType, eventType));
        }

        public dynamic GetSagaPersister(Type sagaType)
        {
            return _resolver.Resolve(typeof(ISaveSagaState<>).MakeGenericType(sagaType));
        }
    }
}