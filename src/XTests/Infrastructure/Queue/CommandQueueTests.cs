using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CavemanTools.Infrastructure;
using CavemanTools.Infrastructure.MessagesBus;
using CavemanTools.Logging;
using Moq;
using XTests.Infrastructure.ServiceBus;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.Queue
{
    public class CommandQueueTests:IDisposable
    {
        private Stopwatch _t = new Stopwatch();
        private CommandsQueue _q;
        private Mock<ISaveQueueState> _storage;
        private Mock<IDispatchCommands> _dispacther;

        public CommandQueueTests()
        {
            LogSetup.ToConsole();
            _storage = new Mock<ISaveQueueState>();
            _dispacther = new Mock<IDispatchCommands>();
            _q = new CommandsQueue(_storage.Object,_dispacther.Object,LogHelper.DefaultLogger);
        }

        [Fact]
        public void bydefault_queue_is_stopped()
        {
            Assert.True(_q.IsPaused);
        }

        [Fact]
        public void any_queued_item_is_saved()
        {
            _q.Queue(new MyCommand());
            _storage.Verify(d=>d.Save(It.IsAny<QueueItem>()),Times.Once());
        }

        [Fact]
        public void queued_item_from_storage_is_executed()
        {
            var item = new QueueItem(new MyCommand());
            _storage.Setup(d => d.GetItems(It.IsAny<DateTime>())).Returns(new[] {item});
            _q.PollingInterval = TimeSpan.FromMilliseconds(400);
            _q.Start();
            Thread.Sleep(TimeSpan.FromSeconds(.5));
            _dispacther.Verify(d => d.Send(It.IsAny<ICommand>()), Times.Once());
            _storage.Verify(d=>d.ItemWasExecuted(item.Id));
        }

        [Fact]
        public void items_are_not_executed_before_their_time()
        {
            var item = new QueueItem(new MyCommand()){ExecuteAt = DateTime.UtcNow.Add(TimeSpan.FromDays(1))};
            var item2 = new QueueItem(new MyCommand());
            _storage.Setup(d => d.GetItems(It.IsAny<DateTime>())).Returns(new[] { item,item2 });
            _q.PollingInterval = TimeSpan.FromMilliseconds(400);
            _q.Start();
            Thread.Sleep(TimeSpan.FromSeconds(.5));
            _dispacther.Verify(d => d.Send(It.IsAny<ICommand>()), Times.Once());
            _storage.Verify(d => d.ItemWasExecuted(item2.Id),Times.Once());
        }


        class FakeStorage:ISaveQueueState
        {
            List<QueueItem> _list= new List<QueueItem>();
            private object _sync = new Object();

            public void Save(QueueItem item)
            {
                lock (_sync)
                {
                    _list.Add(item);
                }
                
            }

            public void ItemWasExecuted(Guid id)
            {
                lock (_sync)
                {
                    _list.RemoveAll(d => d.Id == id);
                }
                
            }

            public IEnumerable<QueueItem> GetItems(DateTime date)
            {
                lock (_sync)
                {
                    return _list.Where(d => d.ShouldBeExecuted).ToArray();    
                }
                
            }
        }

        [Fact(Skip = "takes too long")]
        public void queue_multiple()
        {
            var q = new CommandsQueue(new FakeStorage(), _dispacther.Object, LogHelper.DefaultLogger);

            q.Start();
            q.PollingInterval = TimeSpan.FromMilliseconds(1000);

            var w1 = Task.Factory.StartNew(() =>
                                               {
                                                   Thread.Sleep(100);
                                                   q.Queue(new MyCommand(){Test = 1},TimeSpan.FromMilliseconds(2000));
                                               });
            var w2 = Task.Factory.StartNew(() =>
                                               {
                                                   
                                                   Thread.Sleep(1000);
                                                   q.Queue(new MyCommand() {Test = 2});
                                               });
            Task.WaitAll(w1, w2);
            Thread.Sleep(2000);
            _dispacther.Verify(d=>d.Send(It.IsAny<ICommand>()),Times.Exactly(2));
        }

        [Fact]
        public void paused_queue_doesnt_process_anything()
        {
            _q.Stop();
            _q.PollingInterval = TimeSpan.FromMilliseconds(1);
            _q.Queue(new MyCommand());
            Thread.Sleep(100);
            _storage.Verify(d => d.Save(It.IsAny<QueueItem>()),Times.Once());
            _storage.Verify(d=>d.GetItems(It.IsAny<DateTime>()),Times.Never());
            _q.Start();
            Thread.Sleep(15);
            _storage.Verify(d => d.GetItems(It.IsAny<DateTime>()), Times.AtLeastOnce());
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }

        public void Dispose()
        {
            _q.Dispose();
        }
    }
}