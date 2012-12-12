using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CavemanTools.Infrastructure.MessagesBus;
using CavemanTools.Logging;
using Timer = System.Timers.Timer;

namespace CavemanTools.Infrastructure
{
    public class CommandsQueue:IDisposable, IQueueCommands
    {
        private readonly ISaveQueueState _storage;
        private readonly IDispatchCommands _dispatcher;
        private readonly ILogWriter _logger;

        public CommandsQueue(ISaveQueueState storage,IDispatchCommands dispatcher,ILogWriter logger=null)
        {
            _storage = storage;
            _dispatcher = dispatcher;
            _logger = logger??NullLogger.Instance;
            IsPaused = true;
            SetupTimer();
            PollingInterval = TimeSpan.FromSeconds(1);
        }

        private void SetupTimer()
        {
            _timer = new Timer();
            _timer.Enabled = false;            
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach(var item in _storage.GetItems(DateTime.UtcNow))
            {
               if (item.ShouldBeExecuted)
               {
                   _q.Add(item);
                   _logger.Debug("[Caveman Queue] Added command '{0}' to be executed at {1}", item.Command,item.ExecuteAt);
               }
            }
            
        }

        private TimeSpan _pollingInterval;

        /// <summary>
        /// Time period to check for new items
        /// </summary>
        public TimeSpan PollingInterval
        {
            get { return _pollingInterval; }
            set
            {
                _pollingInterval = value;
                var tt = _timer.Enabled;
                _timer.Enabled = false;
                _timer.Interval = value.TotalMilliseconds;
                _timer.Enabled = tt;
            }
        }

        public bool IsPaused { get; private set; }

       public void Queue(ICommand cmd)
       {
           Queue(cmd,DateTime.UtcNow);
       }

        public void Queue(ICommand cmd,TimeSpan afterInterval)
        {
            Queue(cmd,DateTime.UtcNow.Add(afterInterval));
        }

        public void Queue(ICommand command,DateTime when)
        {
            command.MustNotBeNull("command");
            var item = new QueueItem(command) {ExecuteAt = when};
            _storage.Save(item);
            _logger.Debug("[Caveman Queue] Command '{0}' saved",command);
        }

     
        public void Start()
        {
            IsPaused = false;
            _timer.Start();
            _worker = Task.Factory.StartNew(() =>
            {
                _logger.Info("[Caveman Queue] Worker thread started");

                while (!IsPaused)
                {
                    try
                    {
                        QueueItem tsk = null;
                        if (!_q.TryTake(out tsk))
                        {
                            continue;
                        }
                      
                        ProcessItem(tsk);
                    }
                    catch (InvalidOperationException)
                    {
                        //underlying collection was modified, we can live with that
                    }
                }
                _logger.Info("[Caveman Queue] Worker thread finished");
            });
            _logger.Info("[Caveman Queue] Started.");
        }

        private void ProcessItem(QueueItem tsk)
        {
            if (!tsk.ShouldBeExecuted) throw new Exception("Bug");
            _logger.Debug("[Caveman Queue] Executing command '{0}'",tsk.Command.ToString());
            _dispatcher.Send(tsk.Command);
            _storage.ItemWasExecuted(tsk.Id);
            _logger.Debug("[Caveman Queue] Command '{0}' completed", tsk.Command.ToString());
        }

        private Task _worker;
        private bool _disposed;
        BlockingCollection<QueueItem> _q= new BlockingCollection<QueueItem>();
        private Timer _timer;

        public void Stop()
        {
            if (IsPaused) return;
            IsPaused = true;
            _timer.Stop();
            EndWorker();
            _logger.Info("[Caveman Queue] Stopped.");
        }

        void EndWorker()
        {
            if (_worker != null)
            {
                try
                {
                    _worker.Wait(200);
                }
               catch(AggregateException ex)
               {
                   _logger.Error(ex.Flatten().ToString());
               }
               
                finally
                {
                    _worker = null;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Stop();
            _timer.Dispose();
            _q.CompleteAdding();
            _q.Dispose();
            _logger.Debug("[Caveman Queue] Disposed");
        }
    }
}