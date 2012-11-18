using CavemanTools.Infrastructure;
using CavemanTools.Infrastructure.MessagesBus;
using CavemanTools.Infrastructure.MessagesBus.Saga;
using CavemanTools.Logging;
using Moq;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.ServiceBus
{
    public class MyTestSagaData:IHoldSagaState,IIdentifySaga
    {
        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public bool Event1 { get; set; }
        public bool Event2 { get; set; }
        public string SagaCorrelationId { get; set; }
    }

    public class MySagaEvent:AbstractEvent,IIdentifySaga
    {
        public int OrderId { get; set; }
        public string Data { get; set; }
        public string SagaCorrelationId { get; set; }
        public MySagaEvent()
        {
            SourceId = Guid.Empty;
            SagaCorrelationId = "test";
        }
    }

    public class MySagaOtherEvent:AbstractEvent,IIdentifySaga
    {
        public string SagaCorrelationId { get; set; }
        public int OrderId { get; set; }
        public bool IsOk { get; set; }

        public MySagaOtherEvent()
        {
            SourceId = Guid.Empty;
            SagaCorrelationId = "test";
        }
    }



    public class SagaTests:Saga<MyTestSagaData>
        ,ISagaStartedBy<MySagaEvent>
        ,ISagaStartedBy<MySagaOtherEvent>
    {
        private Stopwatch _t = new Stopwatch();
        private Mock<IStoreMessageBusState> _storage;
        private LocalMessageBus _bus;
        private Mock<IResolveSagaRepositories> _resolver;
        private Inner _child;
        
        public SagaTests()
        {
            LogSetup.ToConsole();
            _child = new Inner(this);

            _storage = new Mock<IStoreMessageBusState>();
            _resolver = new Mock<IResolveSagaRepositories>();
            _sagaStore = new Mock<IGenericSagaRepository>();

            _resolver.Setup(r => r.GetGenericRepository()).Returns(_sagaStore.Object);
          
            _bus = new LocalMessageBus(_storage.Object, LogHelper.DefaultLogger, _resolver.Object);
            
            
            Setup();
        }

        private MyTestSagaData _data= new MyTestSagaData();
        private Mock<IGenericSagaRepository> _sagaStore;

        public class Inner:ISubscribeToEvent<MySagaEvent>,IFindSaga<MyTestSagaData>.Using<MySagaEvent>,ISaveSagaState<MyTestSagaData>
        {
            private readonly SagaTests _parent;

            public Inner(SagaTests parent)
            {
                _parent = parent;
            }

            public void Handle(MySagaEvent evnt)
            {
                _parent.InnerHandlerCalled = true;
            }

            public MyTestSagaData FindSaga(MySagaEvent evnt)
            {
                if (evnt.SagaCorrelationId == "test") return _parent._data;
                return new MyTestSagaData();                
            }

            public void Save(MyTestSagaData saga)
            {
                _parent.SagaSaved = true;
            }
        }

        public bool InnerHandlerCalled { get; set; }
        public bool SagaSaved { get; set; }
        void Setup()
        {
            _bus.RegisterHandler(typeof (MySagaEvent), this);
            _bus.RegisterHandler(typeof (MySagaEvent),_child);
            _bus.RegisterHandler(typeof (MySagaOtherEvent), this);

            _sagaStore.Setup(r => r.GetSaga("test", It.IsAny<Type>())).Returns(_data);
            _sagaStore.Setup(r => r.GetSaga(null, It.IsAny<Type>())).Returns(new MyTestSagaData());
            
            _sagaStore.Setup(r => r.Save(It.IsAny<IIdentifySaga>()));
        }

        void UseCustomPersister()
        {
            _resolver.Setup(r => r.GetSagaLocator(typeof(MyTestSagaData), typeof(MySagaEvent))).Returns(_child);
            _resolver.Setup(r => r.GetSagaPersister(typeof(MyTestSagaData))).Returns(_child);
        }

        [Fact]
        public void inner_event_is_handled()
        {
            Assert.False(InnerHandlerCalled);
            _bus.Publish(new MySagaEvent());
            Assert.True(InnerHandlerCalled);

        }

        [Fact]
        public void event_starts_saga()
        {
            Assert.False(_data.Event1);
            _bus.Publish(new MySagaEvent());
            Assert.True(_data.Event1);
            Assert.False(_data.Event2);
        }

        [Fact]
        public void both_saga_and_event_are_handled()
        {
            Assert.False(InnerHandlerCalled);
            Assert.False(_data.Event1);
            _bus.Publish(new MySagaEvent());
            Assert.True(InnerHandlerCalled);
            Assert.True(_data.Event1);
        }

        [Fact]
        public void saga_is_automatically_saved()
        {
            _bus.Publish(new MySagaEvent());
            _sagaStore.Verify(d=>d.Save(_data),Times.Once());
        }

        [Fact]
        public void saga_is_completed()
        {
            Assert.False(_data.IsCompleted);
            Assert.False(_data.Event1);
            Assert.False(_data.Event2);
            _bus.Publish(new MySagaEvent());
            _bus.Publish(new MySagaOtherEvent());
            Assert.True(_data.IsCompleted);
            Assert.True(_data.Event1);
            Assert.True(_data.Event2);
        }

        [Fact]
        public void new_saga_is_created()
        {
            _bus.Publish(new MySagaEvent(){SagaCorrelationId = null});
            Assert.False(_data.Event1);
            _bus.Publish(new MySagaEvent());
            Assert.True(_data.Event1);
        }

        [Fact]
        public void custom_saga_locator_is_used()
        {
            UseCustomPersister();
            _bus.Publish(new MySagaEvent());
            Assert.True(_data.Event1);
            _resolver.Verify(d=>d.GetGenericRepository(),Times.Never());
            Assert.True(SagaSaved);
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }

        public void Handle(MySagaEvent evnt)
        {
            SagaState.Event1 = true;
            SagaState.SagaCorrelationId = "test";
            TryToComplete();
        }

        public void Handle(MySagaOtherEvent evnt)
        {
            SagaState.Event2 = true;
            SagaState.SagaCorrelationId = "test";
            TryToComplete();
        }

        void TryToComplete()
        {
            if (SagaState.Event1 && SagaState.Event2) MarkAsComplete();
        }
    }
}