using System;
using System.Collections.Generic;
using System.Linq;

namespace CavemanTools.Infrastructure.MessagesBus
{
    /// <summary>
    /// Use this for testing only
    /// </summary>
    public class InMemoryBusStorage:IStoreMessageBusState
    {
       Dictionary<string,IMessage> _items= new Dictionary<string, IMessage>();
       
        public void StoreMessageInProgress(IMessage cmd)
        {
            var key = string.Join("|", cmd.ToDictionary().Where(d=>d.Key!="Id" && d.Key!="TimeStamp" && d.Key!="SourceId").Select(d => d.Key + d.Value));
            try
            {
                _items.Add(key,cmd);
            }
            catch (ArgumentException)
            {
                throw new DuplicateMessageException();
            }
        }
      
        public void MarkMessageCompleted(Guid id)
        {
            var commit = _items.First(d => d.Value.Id == id).Key;
            _items.Remove(commit);
        }

        public void MarkMessageFailed(Guid id)
        {
            
        }

      
        public IEnumerable<IMessage> GetUncompletedMessages(int dd)
        {
            return _items.Select(d => d.Value);
        }

        public void Cleanup()
        {
            throw new NotImplementedException();            
        }

        public void EnsureStorage()
        {
            
        }
    }

   
    public class MessageCommit
    {
        public long Id { get; set; }
        public Guid MessageId { get; set; }
        public DateTime CommittedAt { get; set; }
        public byte[] Body { get; set; }
        public int State { get; set; }
        public string UniqueConstraint { get; set; }
    }
}