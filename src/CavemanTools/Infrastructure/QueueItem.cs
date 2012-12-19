using System;
using CavemanTools.Infrastructure.MessagesBus;

namespace CavemanTools.Infrastructure
{
    public class QueueItem
    {
        public Guid Id { get; set; }
        public QueueItem(ICommand command)
        {
            command.MustNotBeNull();
            Id = Guid.NewGuid();
            Command = command;
        }
        public ICommand Command { get; private set; }
        /// <summary>
        /// Gets or sets the UTC time to execute the command
        /// </summary>
        public DateTime ExecuteAt { get; set; }

        public bool ShouldBeExecuted
        {
            get { return DateTime.UtcNow.Ticks >= ExecuteAt.Ticks; 
            }
        }

        public override string ToString()
        {
            return string.Format("Queued Item '{0}' with command '{1}' Id: {2}",Id,Command.ToString(),Command.Id);
        }
    }
}