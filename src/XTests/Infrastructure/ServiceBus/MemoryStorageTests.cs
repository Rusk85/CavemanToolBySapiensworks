using System.Linq;
using CavemanTools.Infrastructure.MessagesBus;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.ServiceBus
{
    public class MemoryStorageTests
    {
        private Stopwatch _t = new Stopwatch();
        private InMemoryBusStorage _storage;
        private MyCommand _cmd;

        public MemoryStorageTests()
        {
            _storage = new InMemoryBusStorage();
            _cmd = new MyCommand() {Test = 2};
        }

        [Fact]
        public void add_message()
        {
            _storage.StoreMessageInProgress(_cmd);
            _storage.StoreMessageInProgress(new MyCommand(){Test = 56});
            var all=_storage.GetUncompletedMessages(3);
            Assert.Equal(2,all.Count());
        }

        [Fact]
        public void complete_message()
        {
            _storage.StoreMessageInProgress(_cmd);
            _storage.MarkMessageCompleted(_cmd.Id);
            var all = _storage.GetUncompletedMessages(3);
            Assert.Equal(0, all.Count());
        }

        [Fact]
        public void duplicate_mesage_throws()
        {
            _storage.StoreMessageInProgress(_cmd);
            Assert.Throws<DuplicateMessageException>(() => _storage.StoreMessageInProgress(new MyCommand() {Test = 2}));
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}