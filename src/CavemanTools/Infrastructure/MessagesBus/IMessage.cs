using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IMessage
    {
        Guid Id { get; }
        DateTime TimeStamp { get; }
        Guid? SourceId { get; set; }
    }
}